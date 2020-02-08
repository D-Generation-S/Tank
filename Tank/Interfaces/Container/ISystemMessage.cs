using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tank.Interfaces.Container
{
    interface ISystemMessage
    {
        string Sender { get;  }

        string Target { get; }

        object Data { get;  }

        /// <summary>
        /// This will check if the data is from a specific type
        /// </summary>
        /// <param name="type">The data type to check against</param>
        /// <returns></returns>
        bool IsDataType(Type type);
    }
}
