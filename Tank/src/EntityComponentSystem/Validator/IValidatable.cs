using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tank.src.Interfaces.EntityComponentSystem.Manager;

namespace Tank.src.EntityComponentSystem.Validator
{
    interface IValidatable
    {
        bool IsValidEntity(uint entityId, IEntityManager entityManager);
    }
}
