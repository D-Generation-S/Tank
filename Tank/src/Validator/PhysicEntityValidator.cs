using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tank.src.Components;
using Tank.src.EntityComponentSystem.Validator;
using Tank.src.Interfaces.EntityComponentSystem.Manager;

namespace Tank.src.Validator
{
    class PhysicEntityValidator : IValidatable
    {
        /// <summary>
        /// This method will check if the class is a valid physic entity
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="entityManager"></param>
        /// <returns></returns>
        public bool IsValidEntity(uint entityId, IEntityManager entityManager)
        {
            bool valid = entityManager.HasComponent(entityId, typeof(PlaceableComponent));
            valid &= entityManager.HasComponent(entityId, typeof(MoveableComponent));
            return valid;
        }
    }
}
