using DebugFramework.DataTypes;
using DebugFramework.Streaming.Package;
using System.Net;

namespace DebugFramework.Streaming.Clients.Communication
{
    public class CommunicationPackage<T> where T : BaseDataType
    {
        public IPEndPoint Sender { get; }
        public UdpPackage<T> UdpPackage { get; }

        public CommunicationPackage(IPEndPoint sender, UdpPackage<T> udpPackage)
        {
            Sender = sender;
            UdpPackage = udpPackage;
        }
    }
}
