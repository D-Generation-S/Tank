using DebugFramework.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DebugFramework
{
    public class StreamingDataTypeManager
    {
        private readonly List<BaseDataType> dataStorage;

        public StreamingDataTypeManager()
        {
            dataStorage = new List<BaseDataType>();
        }

        public T GetDataType<T>() where T : BaseDataType
        {
            T data = (T)dataStorage.FirstOrDefault(baseData => baseData.GetType() == typeof(T));
            if (data != null)
            {
                dataStorage.Remove(data);
            }
            return data == null ? Activator.CreateInstance<T>() : data;

        }

        public void ReturnBaseData(BaseDataType dataToReturn)
        {
            if (dataStorage == null)
            {
                return;
            }
            dataStorage.Add(dataToReturn);
        }
    }
}
