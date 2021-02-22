using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tank.Components;
using Tank.src.Interfaces.EntityComponentSystem.Manager;

namespace Tank.Validator
{
    /// <summary>
    /// This class will validate all the entites which are maps
    /// </summary>
    class MapValidator : RenderableEntityValidator
    {
        /// <inheritdoc/>
        public override bool IsValidEntity(uint entityId, IEntityManager entityManager)
        {
            bool valid = base.IsValidEntity(entityId, entityManager);
            valid &= entityManager.HasComponent(entityId, typeof(MapComponent));
            return valid;
        }
    }
}
