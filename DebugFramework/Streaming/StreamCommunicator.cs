using DebugFramework.DataTypes;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace DebugFramework.Streaming
{
    /// <summary>
    /// Class to use for stream communication
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class StreamCommunicator<T> where T : PipeStream
    {
        /// <summary>
        /// The seperator used for the message to send
        /// </summary>
        private const string MESSAGE_SEPERATOR = ";";

        /// <summary>
        /// Data left over from last read operation
        /// </summary>
        private string leftOverData;

        /// <summary>
        /// Write the data to the pipe stream
        /// </summary>
        /// <param name="stream">The stream to use for writing</param>
        /// <param name="data">The data which should be written into the stream</param>
        /// <returns>True if writing was successful</returns>
        private bool WriteToStream(T stream, string data)
        {
            if (stream == null || !stream.IsConnected || !stream.CanWrite)
            {
                return false;
            }
            data += MESSAGE_SEPERATOR;
            byte[] bytes = Encoding.ASCII.GetBytes(data);
            stream.Write(bytes, 0, bytes.Length);
            stream.Flush();
            return true;
        }

        /// <summary>
        /// Write a pipe base data object into the stream
        /// </summary>
        /// <typeparam name="O"></typeparam>
        /// <param name="stream">The pipe stream used for writing the data into</param>
        /// <param name="data">The data which should be written</param>
        /// <returns>True if writing was successful</returns>
        public bool WriteToStream<O>(T stream, O data) where O : BaseDataType
        {
            string writeData = JsonSerializer.Serialize(data);
            return WriteToStream(stream, writeData);
        }

        public async Task<bool> WriteToStreamAsync<O>(T stream, O data) where O : BaseDataType
        {
            while (!stream.CanWrite && !stream.IsConnected)
            {
                Thread.Sleep(100);
            }
            return await Task.Run(() => WriteToStream<O>(stream, data));
        }


        /// <summary>
        /// Write a pipe base data object into the stream
        /// </summary>
        /// <typeparam name="O"></typeparam>
        /// <param name="stream">The pipe stream used for writing the data into</param>
        /// <param name="data">The data which should be written</param>
        /// <param name="dataConversionMethod">Method used to convert the data</param>
        /// <returns></returns>
        public bool WriteToStream<O>(T stream, O data, Func<O, string> dataConversionMethod) where O : BaseDataType
        {
            return dataConversionMethod == null ? false : WriteToStream(stream, dataConversionMethod(data));
        }

        /// <summary>
        /// Read the data in the stream
        /// </summary>
        /// <param name="stream">The stream to read data from</param>
        /// <returns>The data read from the stream</returns>
        private string ReadStreamString(T stream)
        {
            if (stream == null || !stream.IsConnected || !stream.CanRead)
            {
                return String.Empty;
            }
            stream.ReadMode = PipeTransmissionMode.Byte;
            int lastDataRead = 0;
            byte[] buffer = new byte[1024];
            string lastReadData = string.Empty;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                do
                {
                    try
                    {
                        lastDataRead = stream.Read(buffer, 0, buffer.Length);
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.Message);
                    }
                    memoryStream.Write(buffer, 0, lastDataRead);
                } while (lastDataRead > 0);

                leftOverData = string.Empty;
                string[] data = Array.FindAll(Encoding.ASCII.GetString(memoryStream.ToArray()).Split(MESSAGE_SEPERATOR.ToCharArray()), splitData => !string.IsNullOrEmpty(splitData));
                if (data.Length > 1)
                {
                    leftOverData = data[1];
                }
                return leftOverData += data[0];
            }
        }

        /// <summary>
        /// Read the data in the stream
        /// </summary>
        /// <param name="stream">The stream to read data from</param>
        /// <returns>A pipe base data set</returns>
        public BaseDataType ReadStream(T stream)
        {
            string data = ReadStreamString(stream);

            return string.IsNullOrEmpty(data) ? default : JsonSerializer.Deserialize<BaseDataType>(data);
        }

        /// <summary>
        /// Read the data in the stream
        /// </summary>
        /// <typeparam name="O">The datatype to cast the result to</typeparam>
        /// <param name="stream">The stream to read data from</param>
        /// <returns>The object of type O if casting was possible</returns>
        public O ReadStream<O>(T stream) where O : BaseDataType
        {
            string data = ReadStreamString(stream);
            return string.IsNullOrEmpty(data) ? default : JsonSerializer.Deserialize<O>(data);
        }

        /// <summary>
        /// Cast the data to type to specific type O
        /// </summary>
        /// <typeparam name="O">The data tyoe to cast to</typeparam>
        /// <param name="data">The data which should be casted</param>
        /// <returns></returns>
        public O CastDataType<O>(BaseDataType data) where O : BaseDataType
        {
            return (O)data;
        }

        /// <summary>
        /// Get the class type of the pipe base data
        /// </summary>
        /// <param name="data">The data set to get the type from</param>
        /// <returns>The data type of the object</returns>
        public Type GetDataTypeOfPipeData(BaseDataType data)
        {
            return data == null ? null : Type.GetType(data.AssemblyQualifiedName);
        }

        public async Task WaitForDataInStream(T stream)
        {
            await Task.Run(() =>
            {
                while (stream == null || !stream.IsConnected || stream.InBufferSize == 0)
                {
                    Thread.Sleep(100);
                }
            });
        }

        /// <summary>
        /// Read the stream async
        /// </summary>
        /// <param name="stream">The stream to read from</param>
        /// <returns>A task with the pipe base data</returns>
        public async Task<BaseDataType> ReadStreamAsync(T stream)
        {
            //await WaitForDataInStream(stream);
            return await Task.Run(() => ReadStream(stream));
        }

        /// <summary>
        /// Read the stream async
        /// </summary>
        /// <typeparam name="O">The type of the data to return</typeparam>
        /// <param name="stream">The stream to read from</param>
        /// <returns>A task with the pipe base data</returns>
        public async Task<O> ReadStreamAsync<O>(T stream) where O : BaseDataType
        {
            //await WaitForDataInStream(stream);
            return await Task.Run(() => ReadStream<O>(stream));

        }
    }
}
