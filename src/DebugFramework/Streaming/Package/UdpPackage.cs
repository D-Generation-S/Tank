using DebugFramework.DataTypes;
using System;
using System.Collections.Generic;

namespace DebugFramework.Streaming.Package
{
    public class UdpPackage : BaseDataPackage
    {
        private const int CUSTOM_HEADER_SIZE = sizeof(uint);

        private uint packageNumber;

        protected override int GetCustomHeaderSizeInNumberOfBytes()
        {
            return CUSTOM_HEADER_SIZE;
        }

        protected override byte[] GetCustomHeader()
        {
            List<byte> dataStream = new List<byte>();
            dataStream.AddRange(BitConverter.GetBytes(packageNumber));
            return dataStream.ToArray();
        }

        protected override bool ParseCustomHeader(byte[] customHeaderBytes)
        {
            if (customHeaderBytes.Length != CUSTOM_HEADER_SIZE)
            {
                return false;
            }
            packageNumber = BitConverter.ToUInt32(customHeaderBytes, 0);
            return true;
        }

        public void Init<T>(uint packageNumber, DataIdentifier identifier, T payload) where T : BaseDataType
        {
            this.packageNumber = packageNumber;
            base.Init(identifier, payload);
        }
    }
}
