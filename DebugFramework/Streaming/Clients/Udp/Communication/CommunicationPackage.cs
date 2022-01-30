using DebugFramework.DataTypes;
using DebugFramework.Streaming.Package;
using System;
using System.Net;

namespace DebugFramework.Streaming.Clients.Udp.Communication
{
    public class CommunicationPackage
    {
        public IPEndPoint Sender { get; }
        public UdpPackage UdpPackage { get; }

        public CommunicationPackage(IPEndPoint sender, UdpPackage udpPackage)
        {
            Sender = sender;
            UdpPackage = udpPackage;
        }

        public Type GetPackageType()
        {
            if (UdpPackage == null)
            {
                return null;
            }
            BaseDataType baseData = UdpPackage.GetPayload();
            return baseData == null ? null : baseData.GetRealType();
        }

        public T GetPackageContent<T>() where T : BaseDataType
        {
            if (UdpPackage == null)
            {
                return default;
            }
            return UdpPackage.GetPayload<T>();
        }
    }
}
