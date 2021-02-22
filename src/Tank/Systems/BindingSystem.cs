using Microsoft.Xna.Framework;
using Tank.Components;
using Tank.Validator;

namespace Tank.Systems
{
    /// <summary>
    /// System to bind entities together
    /// </summary>
    class BindingSystem : AbstractSystem
    {
        /// <summary>
        /// Create a new instance of the binding system
        /// </summary>
        public BindingSystem() : base()
        {
            validators.Add(new BindEntityValidator());
        }

        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            foreach (uint entity in watchedEntities)
            {
                PlaceableComponent basePosition = entityManager.GetComponent<PlaceableComponent>(entity);
                BindComponent binding = entityManager.GetComponent<BindComponent>(entity);
                if (binding == null || !binding.PositionBound || !entityManager.HasComponent(binding.BoundEntityId, typeof(PlaceableComponent)))
                {
                    continue;
                }

                PlaceableComponent bindingPosition = entityManager.GetComponent<PlaceableComponent>(binding.BoundEntityId);
                if (binding.Source)
                {
                    basePosition.Position = bindingPosition.Position + binding.Offset;
                    continue;
                }

                bindingPosition.Position = basePosition.Position + binding.Offset;
            }
        }
    }
}
