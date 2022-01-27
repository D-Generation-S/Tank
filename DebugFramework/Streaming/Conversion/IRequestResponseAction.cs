using DebugFramework.DataTypes;
using System;
using System.IO.Pipes;

namespace DebugFramework.Streaming.Conversion
{
    /// <summary>
    /// Response action for a specific data type
    /// </summary>
    /// <typeparam name="S">The stream type to use for the response</typeparam>
    public interface IRequestResponseAction<S> where S : PipeStream
    {
        bool ResponseToRequest(Type dataType, BaseDataType requestData, S pipeStream, StreamCommunicator<S> communicator);
    }
}
