using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Tank.Components;
using Tank.src.Systems;
using Tank.Validator;

namespace Tank.Systems
{
    class BindingSystem : AbstractSystem
    {
        public BindingSystem() : base()
        {
            validators.Add(new BindEntityValidator());
        }

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
                if (binding.Target)
                {
                    basePosition.Position = bindingPosition.Position + binding.Offset;
                    continue;
                }

                bindingPosition.Position = basePosition.Position + binding.Offset;
            }
        }
    }
}
