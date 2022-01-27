using DebugFramework.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace DebugFramework.Streaming.Package
{
    public class UdpPackage<T> where T : BaseDataType
    {
        private const int HEADER_SIZE = 17;

        private uint packageNumber;

        protected bool isCompressed;

        private int checksumLength => checksum.Length;
        private int payloadLength => payload.Length;
        private DataIdentifier dataIdentifier;
        private bool packageComplete;

        private string checksum;
        private string payload;

        public virtual void Init(uint packageNumber, DataIdentifier identifier, T payload)
        {
            this.packageNumber = packageNumber;
            dataIdentifier = identifier;
            this.payload = CreatePayloadString(JsonSerializer.Serialize(payload));
            checksum = CreateChecksum();
            packageComplete = true;
            isCompressed = false;
        }

        public void Init(byte[] dataStream)
        {
            packageNumber = BitConverter.ToUInt32(dataStream, 0);
            int identifier = BitConverter.ToInt32(dataStream, 4);
            isCompressed = BitConverter.ToBoolean(dataStream, 8);

            dataIdentifier = (DataIdentifier)identifier;

            int checksumLength = BitConverter.ToInt32(dataStream, 9);
            int payloadLength = BitConverter.ToInt32(dataStream, 13);

            payload = string.Empty;
            checksum = string.Empty;

            packageComplete = false;

            if (dataStream.Length != HEADER_SIZE + checksumLength + payloadLength)
            {
                return;
            }

            packageComplete = true;

            byte[] checksumBytes = dataStream.Skip(HEADER_SIZE).Take(checksumLength).ToArray();
            byte[] payloadBytes = dataStream.Skip(HEADER_SIZE + checksumLength).Take(payloadLength).ToArray();

            checksum = Encoding.UTF8.GetString(checksumBytes);
            payload = Encoding.UTF8.GetString(payloadBytes);
        }

        public DataIdentifier GetIdentifier()
        {
            return dataIdentifier;
        }

        public uint GetPackageNumber()
        {
            return packageNumber;
        }

        public bool PackageIsComplete()
        {
            return packageComplete;
        }

        public bool PayloadIsFine()
        {
            return checksum == CreateChecksum(payload);
        }

        private string CreateChecksum()
        {
            return CreateChecksum(payload);
        }

        protected virtual string CreatePayloadString(string baseString)
        {
            return baseString;
        }

        protected virtual string GetPayloadString()
        {
            return payload;
        }

        private string CreateChecksum(string dataToCheck)
        {
            string returnChecksum = string.Empty;
            using (MD5 md5 = MD5.Create())
            {
                byte[] md5Hash = md5.ComputeHash(Encoding.UTF8.GetBytes(dataToCheck));
                returnChecksum = BitConverter.ToString(md5Hash).Replace("-", string.Empty);
            }
            return returnChecksum;
        }

        public byte[] GetDataStream()
        {
            List<byte> dataStream = new List<byte>();

            dataStream.AddRange(BitConverter.GetBytes(packageNumber));
            dataStream.AddRange(BitConverter.GetBytes((int)dataIdentifier));
            dataStream.AddRange(BitConverter.GetBytes(isCompressed));
            dataStream.AddRange(BitConverter.GetBytes(checksumLength));
            dataStream.AddRange(BitConverter.GetBytes(payloadLength));
            dataStream.AddRange(Encoding.UTF8.GetBytes(checksum));
            dataStream.AddRange(Encoding.UTF8.GetBytes(payload));

            return dataStream.ToArray();
        }

        public BaseDataType GetBasePayload()
        {
            return JsonSerializer.Deserialize<BaseDataType>(GetPayloadString());
        }

        public O GetPayload<O>() where O : BaseDataType
        {
            return JsonSerializer.Deserialize<O>(GetPayloadString());
        }
    }
}
