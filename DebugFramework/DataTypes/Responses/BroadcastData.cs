using System.Text.Json.Serialization;

namespace DebugFramework.DataTypes.Responses
{
    public class BroadcastData : BaseDataType
    {
        [JsonPropertyName("name")]
        public string ServerName { get; set; }

        [JsonPropertyName("ip")]
        public string IpAddress { get; set; }

        [JsonPropertyName("uPort")]
        public int UpdatePort { get; set; }

        [JsonPropertyName("cRPort")]
        public int CommunicationRecievePort { get; set; }

        [JsonPropertyName("cSPort")]
        public int CommunicationSendPort { get; set; }
    }
}
