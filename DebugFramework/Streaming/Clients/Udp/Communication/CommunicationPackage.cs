using DebugFramework.DataTypes;
using DebugFramework.Streaming.Package;
using System;
using System.Net;

namespace DebugFramework.Streaming.Clients.Udp.Communication
{
    public class CommunicationPackage
    {
        public IPEndPoint Sender { get; }
        public IDataPackage dataPackage { get; }

        public CommunicationPackage(IPEndPoint sender, IDataPackage udpPackage)
        {
            Sender = sender;
            dataPackage = udpPackage;
        }

        public Type GetPackageType()
        {
            if (dataPackage == null)
            {
                return null;
            }
            BaseDataType baseData = dataPackage.GetPayload();
            return baseData == null ? null : baseData.GetRealType();
        }

        public T GetPackageContent<T>() where T : BaseDataType
        {
            if (dataPackage == null)
            {
                return default;
            }
            return dataPackage.GetPayload<T>();
        }
    }
}
