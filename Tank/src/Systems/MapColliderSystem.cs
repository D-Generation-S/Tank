using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tank.Code.DataContainer;
using Tank.Interfaces.MapGenerators;
using Tank.src.Components;
using Tank.src.Events.PhysicBased;
using Tank.src.Validator;

namespace Tank.src.Systems
{
    class MapColliderSystem : AbstractSystem
    {


        public MapColliderSystem() : base()
        {
            validators.Add(new MapColliderValidator());
            validators.Add(new MapValidator());
        }

        public override void EntityAdded(uint entityId)
        {
            base.EntityAdded(entityId);
            if (validators[1].IsValidEntity(entityId, entityManager))
            {
                MapComponent mapComponent = entityManager.GetComponent<MapComponent>(entityId);
                VisibleComponent visibleComponent = entityManager.GetComponent<VisibleComponent>(entityId);

                IMap map = mapComponent.Map;
                visibleComponent.Texture = map.Image;
                visibleComponent.Source = new Rectangle(0, 0, map.Width, map.Height);
                visibleComponent.Destination = visibleComponent.Source;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            MapComponent mapEntity = GetMapComponent();
            if (mapEntity == null)
            {
                return;
            }

            foreach (uint entityId in watchedEntities)
            {
                if (entityManager.HasComponent(entityId, typeof(MapComponent)))
                {
                    continue;
                }

                PlaceableComponent placeableComponent = entityManager.GetComponent<PlaceableComponent>(entityId);
                ColliderComponent colliderComponent = entityManager.GetComponent<ColliderComponent>(entityId);
                MoveableComponent moveableComponent = entityManager.GetComponent<MoveableComponent>(entityId);

                if (!colliderComponent.MapCollision || moveableComponent.OnGround)
                {
                    continue;
                }

                Position positionToCheck = new Position(
                    colliderComponent.Collider.X + colliderComponent.Collider.Width,
                    colliderComponent.Collider.Y + colliderComponent.Collider.Height
                );
                positionToCheck.X += (int)placeableComponent.Position.X;
                positionToCheck.Y += (int)placeableComponent.Position.Y;

                if (mapEntity.Map.CollissionMap.GetValue(positionToCheck.X, positionToCheck.Y))
                {
                    moveableComponent.OnGround = true;
                    
                    for (int y = 0; y < colliderComponent.Collider.Height * 2; y++)
                    {
                        if (!mapEntity.Map.CollissionMap.GetValue(positionToCheck.X, positionToCheck.Y - y))
                        {
                            int newYPosition = positionToCheck.Y - y;
                            newYPosition -= colliderComponent.Collider.Height;
                            Vector2 Position = new Vector2(placeableComponent.Position.X, newYPosition);
                            
                            VisibleComponent visibleComponent = entityManager.GetComponent<VisibleComponent>(entityId);
                            if (visibleComponent != null)
                            {
                                Position.Y += visibleComponent.Source.Height / 2;
                            }
                            placeableComponent.Position = Position;
                            FireEvent<MapCollisionEvent>(new MapCollisionEvent(entityId, placeableComponent.Position));
                            break;
                        }
                    }
                }
            }
        }

        private MapComponent GetMapComponent()
        {
            foreach (uint entityId in watchedEntities)
            {
                if (entityManager.HasComponent(entityId, typeof(MapComponent)))
                {
                    return entityManager.GetComponent<MapComponent>(entityId);
                }
            }
            return null;
        }
    }
}
