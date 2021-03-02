using System;
using System.Collections.Generic;
using System.Text;
using Tank.Wrapper;

namespace Tank.DataManagement.Loader
{
    interface IDataLoader<T>
    {
        void Init(ContentWrapper contentWrapper);
        T LoadData(string fileName);
    }
}
