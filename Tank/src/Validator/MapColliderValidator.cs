using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tank.src.Components;
using Tank.src.Interfaces.EntityComponentSystem.Manager;

namespace Tank.src.Validator
{
    /// <summary>
    /// This class will calidate all the entites which can collide with the map
    /// </summary>
    class MapColliderValidator : ColliderObjectValidator
    {
        /// <inheritdoc/>
        public override bool IsValidEntity(uint entityId, IEntityManager entityManager)
        {
            bool valid =  base.IsValidEntity(entityId, entityManager);
            valid &= entityManager.HasComponent(entityId, typeof(MoveableComponent));
            return valid;
        }
    }
}
