using DebugFramework.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DebugFramework.Streaming.Package
{
    public abstract class BaseDataPackage : IDataPackage
    {
        protected const int BASE_HEADER_SIZE = sizeof(DataIdentifier) + sizeof(bool) + sizeof(int) * 2;

        private readonly JsonSerializerOptions options;

        private DataIdentifier dataIdentifier;
        protected bool isCompressed;

        private byte[] checksum;
        private byte[] payload;

        private int checksumLength;
        private int payloadLength;

        private bool isPackageComplete;

        public BaseDataPackage()
        {
            options = new JsonSerializerOptions()
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            };
        }

        public void Init(byte[] dataStream)
        {
            isPackageComplete = false;

            int headerSize = BASE_HEADER_SIZE + GetCustomHeaderSizeInBytes();
            byte[] headerBytes = dataStream.Take(headerSize).ToArray();
            ParseHeader(headerBytes);

            byte[] payloadBytes = dataStream.Skip(headerSize).ToArray();
            if (payloadBytes.Length != checksumLength + payloadLength)
            {
                return;
            }
            isPackageComplete = true;
            checksum = payloadBytes.Take(checksumLength).ToArray();
            payload = payloadBytes.Skip(checksumLength).Take(payloadLength).ToArray();
        }

        protected virtual void Init<T>(DataIdentifier identifier, T payload) where T : BaseDataType
        {
            dataIdentifier = identifier;
            this.payload = CreatePayloadData(payload);
            checksum = CreateChecksum(this.payload);

            payloadLength = this.payload.Length;
            checksumLength = checksum.Length;

            isPackageComplete = true;
        }

        public virtual bool IsPayloadFine()
        {
            byte[] createdChecksum = CreateChecksum(payload);
            return ByteArrayIdentical(checksum, createdChecksum);
        }

        protected bool ByteArrayIdentical(byte[] firstArray, byte[] secondArray)
        {
            if (firstArray.Length != secondArray.Length)
            {
                return false;
            }
            bool valid = true;
            for (int i = 0; i < firstArray.Length; i++)
            {
                valid &= firstArray[i] == secondArray[i];
            }
            return valid;
        }

        public virtual bool IsPackageComplete()
        {
            return isPackageComplete;
        }


        public byte[] GetDataStream()
        {
            List<byte> dataStream = new List<byte>();
            dataStream.AddRange(GetHeaderStream());
            dataStream.AddRange(checksum);
            dataStream.AddRange(payload);
            return dataStream.ToArray();
        }

        protected byte[] GetHeaderStream()
        {
            List<byte> dataStream = new List<byte>();
            dataStream.AddRange(BitConverter.GetBytes((int)dataIdentifier));
            dataStream.AddRange(BitConverter.GetBytes(isCompressed));
            dataStream.AddRange(BitConverter.GetBytes(checksumLength));
            dataStream.AddRange(BitConverter.GetBytes(payloadLength));
            dataStream.AddRange(GetCustomHeader());

            return dataStream.ToArray();
        }

        protected abstract int GetCustomHeaderSizeInBytes();

        protected abstract byte[] GetCustomHeader();

        protected virtual byte[] CreatePayloadData<T>(T payload)
        {
            string data = JsonSerializer.Serialize(payload);
            return Encoding.UTF8.GetBytes(data);
        }

        protected virtual byte[] GetPayloadData()
        {
            return payload;
        }

        protected byte[] CreateChecksum(byte[] dataToCheck)
        {
            if (dataToCheck == null || dataToCheck.Length == 0)
            {
                return new byte[0];
            }
            byte[] hash;
            using (MD5 md5 = MD5.Create())
            {
                hash = md5.ComputeHash(dataToCheck);
            }
            return hash;
        }

        protected virtual void ParseHeader(byte[] headerBytes)
        {
            if (headerBytes.Length != BASE_HEADER_SIZE + GetCustomHeaderSizeInBytes())
            {
                return;
            }

            int identifier = BitConverter.ToInt32(headerBytes, 0);
            dataIdentifier = (DataIdentifier)identifier;
            isCompressed = BitConverter.ToBoolean(headerBytes, 4);

            checksumLength = BitConverter.ToInt32(headerBytes, 5);
            payloadLength = BitConverter.ToInt32(headerBytes, 9);

            ParseCustomHeader(headerBytes.Skip(BASE_HEADER_SIZE).ToArray());
        }

        protected abstract void ParseCustomHeader(byte[] customHeaderBytes);

        public BaseDataType GetPayload()
        {
            if (payloadLength == 0)
            {
                return default;
            }
            return JsonSerializer.Deserialize<BaseDataType>(payload);
        }

        public T GetPayload<T>()
        {
            if (payloadLength == 0)
            {
                return default;
            }
            return JsonSerializer.Deserialize<T>(payload);
        }
    }
}
