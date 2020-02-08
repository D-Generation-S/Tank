using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tank.Code.DataContainer.Messages
{
    class GetEntity : DefaultSystemMessage
    {
        public GetEntity(string sender, string target, object data)
            : base(sender, target, data)
        {
        }
    }
}
