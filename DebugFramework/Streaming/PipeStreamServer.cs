using DebugFramework.DataTypes;
using DebugFramework.Streaming.Conversion;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Pipes;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DebugFramework.Streaming
{
    public class PipeStreamServer
    {
        private bool serverRunning;
        private object asyncLock = new object();
        private readonly Dictionary<Type, IRequestResponseAction<NamedPipeServerStream>> responseActions;

        public PipeStreamServer()
        {
            responseActions = new Dictionary<Type, IRequestResponseAction<NamedPipeServerStream>>();
            serverRunning = true;
        }

        /// <summary>
        /// Add all the default response actions
        /// </summary>
        protected virtual void SetupDefaultActions()
        {
            responseActions.Add(typeof(UnknownType), new LambdaResponseAction<NamedPipeServerStream>((type, data, stream, communicator) =>
            {
                return communicator.WriteToStream(stream, new UnknownType());
            }));
        }

        /// <summary>
        /// Add a new response action overwritting the already existing one
        /// </summary>
        /// <param name="typeToResponseTo">The type of data to response to</param>
        /// <param name="responseAction">The action used as a response</param>
        public void AddResponseAction(Type typeToResponseTo, IRequestResponseAction<NamedPipeServerStream> responseAction)
        {
            if (responseActions.ContainsKey(typeToResponseTo))
            {
                responseActions.Remove(typeToResponseTo);
            }
            responseActions.Add(typeToResponseTo, responseAction);
        }

        public async Task StartListenAsync()
        {
            _ = Task.Run(async () =>
            {
                using (NamedPipeServerStream pipeServerStream = new NamedPipeServerStream("DebugEntryStream", PipeDirection.InOut, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous))
                {
                    while (serverRunning)
                    {
                        if (!pipeServerStream.IsConnected)
                        {
                            Console.WriteLine("Waiting for client to connect");
                            pipeServerStream.WaitForConnection();
                            await HandleNewConnectionAsync(pipeServerStream);
                        }
                        Thread.Sleep(100);
                    }

                }
            });

        }

        public void StopListening()
        {
            serverRunning = false;
        }

        /// <summary>
        /// Handle a new connaction
        /// </summary>
        /// <param name="mainStream">The main stream to use</param>
        /// <returns>Nothing</returns>
        public async Task HandleNewConnectionAsync(NamedPipeServerStream mainStream)
        {
            StreamCommunicator<NamedPipeServerStream> mainCommunicator = new StreamCommunicator<NamedPipeServerStream>();
            string newStreamBaseName = GetRandomName(20);
            Debug.WriteLine("Request switch to pipe with name {0}", newStreamBaseName);
            mainCommunicator.WriteToStream(mainStream, new ChangeNamedPipeRequest(newStreamBaseName));
            mainStream.Disconnect();
            _ = Task.Run(async () =>
              {
                  using (NamedPipeServerStream taskInternalStream = new NamedPipeServerStream(newStreamBaseName, PipeDirection.InOut, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous))
                  {
                      StreamCommunicator<NamedPipeServerStream> privateCommunicator = new StreamCommunicator<NamedPipeServerStream>();
                      taskInternalStream.WaitForConnection();
                      Debug.WriteLine("Connected on stream {0}", newStreamBaseName);
                      while (taskInternalStream.IsConnected && taskInternalStream.CanRead && taskInternalStream.CanWrite)
                      {
                          //while (taskInternalStream.InBufferSize == 0)
                          //{
                          //Thread.Sleep(1000);
                          //}

                          BaseDataType baseType = await privateCommunicator.ReadStreamAsync(taskInternalStream);
                          Type dataType = privateCommunicator.GetDataTypeOfPipeData(baseType);

                          lock (asyncLock)
                          {
                              IRequestResponseAction<NamedPipeServerStream> actionToPerform = responseActions.ContainsKey(dataType) ? responseActions[dataType] : responseActions[typeof(UnknownType)];
                              actionToPerform.ResponseToRequest(dataType, baseType, taskInternalStream, privateCommunicator);
                          }
                      }
                  }
              });
        }

        /// <summary>
        /// Get a random string name for a new stream to connect to
        /// </summary>
        /// <param name="length">The lenght of the new random name</param>
        /// <returns>The random name to use</returns>
        private string GetRandomName(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
