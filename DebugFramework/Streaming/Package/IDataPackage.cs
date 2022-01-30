using DebugFramework.DataTypes;

namespace DebugFramework.Streaming.Package
{
    public interface IDataPackage
    {
        void Init(byte[] dataStream);

        byte[] GetDataStream();

        bool IsPayloadFine();

        bool IsPackageComplete();

        BaseDataType GetPayload();

        T GetPayload<T>();
    }
}
