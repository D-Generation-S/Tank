using DebugFramework.Streaming.Package;
using System.Net;

namespace DebugFramework.Streaming.Clients.Udp.Communication
{
    /// <summary>
    /// Class which combined the recieved data and the sender from which the data was recieved from
    /// </summary>
    public class CommunicationPackage
    {
        /// <summary>
        /// The sender who send the data
        /// </summary>
        public IPEndPoint Sender { get; }

        /// <summary>
        /// The data package which was send
        /// </summary>
        public IDataPackage dataPackage { get; }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="sender">Who did send the data package</param>
        /// <param name="udpPackage">The package which was sendet</param>
        public CommunicationPackage(IPEndPoint sender, IDataPackage udpPackage)
        {
            Sender = sender;
            dataPackage = udpPackage;
        }
    }
}
