using System;
using System.Collections.Generic;
using System.Text;
using Tank.Wrapper;

namespace Tank.DataManagement.Loader
{
    abstract class AbstractDataLoader<T> : IDataLoader<T>
    {
        protected ContentWrapper contentWrapper;

        public virtual void Init(ContentWrapper contentWrapper)
        {
            this.contentWrapper = contentWrapper;
        }

        public abstract T LoadData(string fileName);
    }
}
