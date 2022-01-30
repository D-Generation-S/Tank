using DebugFramework.DataTypes;

namespace DebugFramework.Streaming.Package
{
    public class TcpPackage : BaseDataPackage
    {
        private const int CUSTOM_HEADER_SIZE = 0;

        protected override byte[] GetCustomHeader()
        {
            return new byte[0];
        }

        protected override int GetCustomHeaderSizeInNumberOfBytes()
        {
            return CUSTOM_HEADER_SIZE;
        }

        protected override bool ParseCustomHeader(byte[] customHeaderBytes)
        {
            return true;
        }

        public new void Init<T>(DataIdentifier identifier, T payload) where T : BaseDataType
        {
            base.Init(identifier, payload);
        }


    }
}
