using System.Text.Json.Serialization;

namespace DebugFramework.DataTypes.Responses
{
    /// <summary>
    /// Dataset for broadcast data
    /// </summary>
    public class BroadcastData : BaseDataType
    {
        /// <summary>
        /// The name of the server where the broadcast is getting send from
        /// </summary>
        [JsonPropertyName("name")]
        public string ServerName { get; set; }

        /// <summary>
        /// The ip address of the server where the broadcast is getting send from
        /// </summary>
        [JsonPropertyName("ip")]
        public string IpAddress { get; set; }

        /// <summary>
        /// The tcp port where updates are getting send via tcp
        /// </summary>
        [JsonPropertyName("uPort")]
        public int UpdatePort { get; set; }

        /// <summary>
        /// Port to send requests to
        /// </summary>
        [JsonPropertyName("cRPort")]
        public int CommunicationRecievePort { get; set; }

        /// <summary>
        /// Port where awnsers from requests are getting send from
        /// </summary>
        [JsonPropertyName("cSPort")]
        public int CommunicationSendPort { get; set; }
    }
}
