using System;
using System.Collections.Generic;
using System.Text;

namespace Tank.Register
{
    class Register<T> : DictionaryExtended<string, T>
    {
        public int GetPosition(string key)
        {
            if (!Contains(key))
            {
                return -1;
            }

            int position = 0;
            foreach(string currentKey in GetKeys())
            {
                if (currentKey == key)
                {
                    break;
                }
                position++;
            }

            return position;
        }
    }
}
