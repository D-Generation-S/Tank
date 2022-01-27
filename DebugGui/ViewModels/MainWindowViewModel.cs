using DebugFramework.DataTypes;
using DebugFramework.DataTypes.Requests;
using DebugFramework.Streaming;
using DebugFramework.Streaming.Package;
using System;
using System.IO.Pipes;
using System.Windows.Input;

namespace DebugGui.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public string Greeting => "Welcome to Avalonia!";
        private NamedPipeClientStream CommunicationStream;
        private readonly StreamCommunicator<NamedPipeClientStream> communicator;
        public ICommand TestCommand { get; }

        public MainWindowViewModel()
        {
            UdpPackage<ChangeNamedPipeRequest> package = new UdpPackage<ChangeNamedPipeRequest>();
            ChangeNamedPipeRequest changeNamedPipeRequest = new ChangeNamedPipeRequest();
            changeNamedPipeRequest.BaseName = "aaaaaaaaaaaaaaaaaaaaaaaaaaabbbbbbbbbbbbbbbbbbbbbbbbbsssssssssssssssssssssssssssssssss";
            package.Init(0, DataIdentifier.Request, changeNamedPipeRequest);
            byte[] data = package.GetDataStream();
            UdpPackage<ChangeNamedPipeRequest> loadedPackage = new UdpPackage<ChangeNamedPipeRequest>();
            loadedPackage.Init(data);
            if (loadedPackage.PayloadIsFine())
            {
                string typeName = loadedPackage.GetBasePayload().AssemblyQualifiedName;
                Type dataType = Type.GetType(typeName);
                if (dataType == typeof(EntityDumpRequest))
                {
                    EntityDumpRequest loadedRequest = loadedPackage.GetPayload<EntityDumpRequest>();
                }
            }
        }
    }
}
