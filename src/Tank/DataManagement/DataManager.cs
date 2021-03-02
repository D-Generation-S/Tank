using System;
using System.Collections.Generic;
using System.Text;
using Tank.DataManagement.Loader;
using Tank.Wrapper;

namespace Tank.DataManagement
{
    class DataManager<T>
    {
        private readonly IDataLoader<T> dataLoader;

        public DataManager(ContentWrapper contentWrapper, IDataLoader<T> dataLoader)
        {
            dataLoader.Init(contentWrapper);
            this.dataLoader = dataLoader;
        }

        public T GetData(string fileName)
        {
            return dataLoader.LoadData(fileName);
        }
    }
}
