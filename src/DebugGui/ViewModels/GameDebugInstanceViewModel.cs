using DebugFramework.DataTypes.Responses;
using System;

namespace DebugGui.ViewModels
{
    public class GameDebugInstanceViewModel : ViewModelBase
    {
        public string MachineName { get; set; }
        public string IpAddress { get; set; }
        public int Port { get; set; }

        public DateTime LastTimeFound { get; set; }

        public GameDebugInstanceViewModel(BroadcastData broadcastData)
        {
            MachineName = broadcastData.ServerName;
            IpAddress = broadcastData.IpAddress;
            Port = broadcastData.UpdatePort;
            LastTimeFound = DateTime.Now;
        }
    }
}
