using System.IO.Pipes;

namespace DebugFramework.Streaming.Conversion
{
    public class LambdaResponseAction<S> : IRequestResponseAction<S> where S : PipeStream
    {
        /**
        private readonly Func<Type, BaseDataType, S, StreamCommunicator<S>, bool> responseAction;

        public LambdaResponseAction(Func<Type, BaseDataType, S, StreamCommunicator<S>, bool> responseAction)
        {
            this.responseAction = responseAction;
        }

        public bool ResponseToRequest(Type dataType, BaseDataType requestData, S pipeStream, StreamCommunicator<S> communicator)
        {
            return responseAction == null ? false : responseAction(dataType, requestData, pipeStream, communicator);
        }
        */
    }
}
