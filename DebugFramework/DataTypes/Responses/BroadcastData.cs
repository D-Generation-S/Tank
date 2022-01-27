namespace DebugFramework.DataTypes.Responses
{
    public class BroadcastData : BaseDataType
    {
        public string ServerName { get; set; }

        public string IpAddress { get; set; }
        public int UpdatePort { get; set; }
        public int CommunicationPort { get; set; }
    }
}
