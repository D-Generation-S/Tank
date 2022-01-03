using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Tank.Components;
using Tank.Components.Rendering;
using Tank.Components.Tags;
using TankEngine.EntityComponentSystem;

namespace Tank.Builders
{
    class MapDebriBuilder : BaseBuilder
    {
        private readonly Texture2D pixelTexture;

        private readonly Rectangle bounds;

        private readonly Rectangle destination;
        private readonly Rectangle collider;

        public MapDebriBuilder(Texture2D pixelTexture)
        {
            this.pixelTexture = pixelTexture;
            this.bounds = new Rectangle(0, 0, 1, 1);
            this.destination = new Rectangle(0, 0, 4, 4);
            this.collider = new Rectangle(-2, -2, 4, 4);
        }

        public override List<IComponent> BuildGameComponents(object parameter)
        {
            List<IComponent> returnComponents = new List<IComponent>();
            if (parameter is Color color)
            {
                PlaceableComponent placeableComponent = entityManager.CreateComponent<PlaceableComponent>();
                MoveableComponent moveableComponent = entityManager.CreateComponent<MoveableComponent>();
                moveableComponent.Mass = 4;
                VisibleComponent visibleComponent = entityManager.CreateComponent<VisibleComponent>();
                visibleComponent.Texture = pixelTexture;
                visibleComponent.Color = color;
                visibleComponent.Source = bounds;
                visibleComponent.Destination = destination;
                visibleComponent.Source = bounds;
                visibleComponent.DrawMiddle = true;
                ColliderComponent colliderComponent = entityManager.CreateComponent<ColliderComponent>();
                colliderComponent.Collider = collider;
                colliderComponent.FireCollideEvent = true;
                colliderComponent.FireBelow = true;

                returnComponents.Add(placeableComponent);
                returnComponents.Add(moveableComponent);
                returnComponents.Add(visibleComponent);
                returnComponents.Add(colliderComponent);
                returnComponents.Add(entityManager.CreateComponent<AddTerrainTag>());
            }

            return returnComponents;
        }
    }
}
