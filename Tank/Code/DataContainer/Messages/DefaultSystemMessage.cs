using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tank.Interfaces.Container;

namespace Tank.Code.DataContainer.Messages
{
    class DefaultSystemMessage : ISystemMessage
    {
        private readonly string sender;
        public string Sender => sender;

        private readonly string target;
        public string Target => target;

        private readonly object  data;
        public object Data => data;

        public DefaultSystemMessage(string sender, string target, object data)
        {
            this.sender = sender;
            this.target = target;
            this.data = data;
        }

        public bool IsDataType(Type type)
        {
            return data.GetType() == type;
        }
    }
}
