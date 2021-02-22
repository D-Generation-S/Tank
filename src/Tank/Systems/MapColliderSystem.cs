using Microsoft.Xna.Framework;
using Tank.Components;
using Tank.DataStructure;
using Tank.Events.PhysicBased;
using Tank.Interfaces.MapGenerators;
using Tank.Utils;
using Tank.Validator;

namespace Tank.Systems
{
    /// <summary>
    /// This system will create collisions events if some entites hit the ground
    /// </summary>
    class MapColliderSystem : AbstractSystem
    {
        /// <summary>
        /// Create an new instance of this class
        /// </summary>
        public MapColliderSystem() : base()
        {
            validators.Add(new MapColliderValidator());
            validators.Add(new MapValidator());
        }

        /// <summary>
        /// Add a visible component if a map is getting added to this system
        /// </summary>
        /// <param name="entityId">The id of the entity which was added</param>
        protected override void EntityAdded(uint entityId)
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

        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            MapComponent mapEntity = GetMapComponent();
            if (mapEntity == null)
            {
                return;
            }

            updateLocked = true;
            foreach (uint entityId in watchedEntities)
            {
                if (entityManager.HasComponent(entityId, typeof(MapComponent)))
                {
                    continue;
                }

                PlaceableComponent placeableComponent = entityManager.GetComponent<PlaceableComponent>(entityId);
                ColliderComponent colliderComponent = entityManager.GetComponent<ColliderComponent>(entityId);
                MoveableComponent moveableComponent = entityManager.GetComponent<MoveableComponent>(entityId);

                if (!colliderComponent.MapCollision)
                {
                    continue;
                }

                Position positionToCheck = new Position(
                    colliderComponent.Collider.Right,
                    colliderComponent.Collider.Bottom
                );
                Position CenterPosition = new Position(
                    colliderComponent.Collider.Center.X,
                    colliderComponent.Collider.Bottom
                 );
                positionToCheck.Add(placeableComponent.Position);
                CenterPosition.Add(placeableComponent.Position);
                Vector2 newPosition = placeableComponent.Position + moveableComponent.Velocity;
                Vector2 direction = newPosition - placeableComponent.Position;
                if (direction == Vector2.Zero)
                {
                    Position positionBelow = GetPositionBelow(positionToCheck.GetVector2(), colliderComponent.Collider.Height, mapEntity.Map);
                    positionBelow = positionBelow == null ? GetPositionBelow(CenterPosition.GetVector2(), colliderComponent.Collider.Height, mapEntity.Map) : positionBelow;
                    if (positionBelow != null)
                    {
                        FireEvent(new MapCollisionEvent(entityId, positionBelow));
                    }
                }
                Raycast raycast = new Raycast(positionToCheck.GetVector2(), direction, direction.Length() * 3);
                Position[] positions = raycast.GetPositions();
                for (int i = 0; i < positions.Length; i++)
                {
                    Position position = positions[i];
                    if (mapEntity.Map.CollissionMap.GetValue(position.X, position.Y))
                    {
                        FireEvent(new MapCollisionEvent(entityId, position));
                    }
                }
            }
            updateLocked = false;
        }

        /// <summary>
        /// Get the position below
        /// </summary>
        /// <param name="position">The position origin</param>
        /// <param name="colliderHeight">The height of the collider</param>
        /// <param name="map">The map to check agains</param>
        /// <returns>The first position which is a hit</returns>
        private Position GetPositionBelow(Vector2 position, int colliderHeight, IMap map)
        {
            Raycast raycast = new Raycast(position, Vector2.UnitY, colliderHeight);
            Position[] positions = raycast.GetPositions();
            for (int i = 0; i < positions.Length; i++)
            {
                if (map.CollissionMap.GetValue(positions[i]))
                {
                    return positions[i];
                }
            }
            return null;
        }

        /// <summary>
        /// This method will return you the map component from the watched entity
        /// </summary>
        /// <returns>The instance of the map component if present or null</returns>
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
