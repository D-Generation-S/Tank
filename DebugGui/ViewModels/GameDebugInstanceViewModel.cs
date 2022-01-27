using DebugFramework.DataTypes.Responses;

namespace DebugGui.ViewModels
{
    public class GameDebugInstanceViewModel : ViewModelBase
    {
        public string MachineName { get; set; }
        public string IpAddress { get; set; }
        public int Port { get; set; }

        public GameDebugInstanceViewModel()
        {
            MachineName = string.Empty;
            IpAddress = string.Empty;
            Port = -1;
        }

        public GameDebugInstanceViewModel(BroadcastData broadcastData)
        {
            MachineName = broadcastData.ServerName;
            IpAddress = broadcastData.IpAddress;
            Port = broadcastData.UpdatePort;
        }
    }
}
