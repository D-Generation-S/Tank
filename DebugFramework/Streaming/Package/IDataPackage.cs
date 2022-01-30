using DebugFramework.DataTypes;

namespace DebugFramework.Streaming.Package
{
    public interface IDataPackage
    {
        void Init(byte[] dataStream);

        bool ParseHeader(byte[] headerBytes);

        int GetHeaderSize();

        int GetPayloadSize();

        int GetCompletePackageSize();

        byte[] GetDataStream();

        bool IsPayloadFine();

        bool IsPackageComplete();

        BaseDataType GetPayload();

        T GetPayload<T>();
    }
}
