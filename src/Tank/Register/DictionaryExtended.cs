using System.Collections.Generic;
using System.Linq;

namespace Tank.Register
{
    class DictionaryExtended <G, T>
    {
        public int Count => registeredEntries.Count;
        public bool Sealed => isSealed;
        public bool OverrideAllowed => overrideAllowed;


        private Dictionary<G, T> registeredEntries;
        private bool overrideAllowed;
        private bool isSealed;
        public DictionaryExtended()
        {
            registeredEntries = new Dictionary<G, T>();
        }

        public void SetOverride(bool overrideAllowed)
        {
            this.overrideAllowed = overrideAllowed;
        }

        public void SealDictionary()
        {
            isSealed = true;
        }

        public virtual bool Add(G key, T value)
        {
            if (isSealed)
            {
                return false;
            }
            if (!overrideAllowed && Contains(key))
            {
                return false;
            }

            registeredEntries.Add(key, value);
            return true;
        }

        public T GetValue(G key)
        {
            if (!Contains(key))
            {
                return default(T);
            }

            return registeredEntries[key];
        }

        public T GetValue(int position)
        {
            if (!Contains(position))
            {
                return default(T);
            }
            return registeredEntries.ElementAt(position).Value;
        }

        public IEnumerable<G> GetKeys()
        {
            return registeredEntries.Keys;
        }

        public virtual void Remove(G key)
        {
            if (isSealed)
            {
                return;
            }
            registeredEntries.Remove(key);
        }

        public bool Contains(G key)
        {
            return registeredEntries.ContainsKey(key);
        }

        public bool Contains(int position)
        {
            if (position < 0 || position > registeredEntries.Count - 1)
            {
                return false;
            }

            return true;
        }
    }
}
