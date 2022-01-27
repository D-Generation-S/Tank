using DebugFramework.Streaming;

namespace DebugGui.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ViewModelBase WindowContent { get; private set; }

        public string Greeting => "Welcome to Avalonia!";

        public MainWindowViewModel()
        {
            UdpServer server = new UdpServer();
            server.StartServer();
            WindowContent = new MainDebugViewModel();
        }
    }
}
