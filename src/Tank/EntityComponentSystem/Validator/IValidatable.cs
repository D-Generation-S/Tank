using Tank.Interfaces.EntityComponentSystem.Manager;

namespace Tank.EntityComponentSystem.Validator
{
    /// <summary>
    /// This interface describes a validator for systems
    /// </summary>
    interface IValidatable
    {
        /// <summary>
        /// Is an entity valid
        /// </summary>
        /// <param name="entityId">The id of the entity to check</param>
        /// <param name="entityManager">The manager instance to use for checking</param>
        /// <returns>True if an entity is valid</returns>
        bool IsValidEntity(uint entityId, IEntityManager entityManager);
    }
}
