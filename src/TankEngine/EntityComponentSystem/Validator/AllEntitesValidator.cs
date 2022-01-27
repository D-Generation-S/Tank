using TankEngine.EntityComponentSystem.Manager;

namespace TankEngine.EntityComponentSystem.Validator
{
    /// <summary>
    /// All entites are allowed for this one
    /// </summary>
    public class AllEntitesValidator : IValidatable
    {
        /// <inheritdoc/>
        public bool IsValidEntity(uint entityId, IEntityManager entityManager)
        {
            return true;
        }
    }
}
