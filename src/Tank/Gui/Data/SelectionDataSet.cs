using System;
using System.Collections.Generic;
using System.Text;

namespace Tank.Gui.Data
{
    class SelectionDataSet
    {
        public string DisplayText { get; }
        private object data;

        public SelectionDataSet(string displayText, object data)
        {
            DisplayText = displayText;
            this.data = data;
        }

        public T GetData<T>()
        {
            Type t = typeof(T);
            try
            {
                return (T)Convert.ChangeType(data, t);
            }
            catch (Exception)
            {
                return default(T);
            }
        }
    }
}
