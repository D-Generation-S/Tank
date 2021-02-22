using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Tank.Components;
using Tank.DataStructure;
using Tank.Events.TerrainEvents;
using Tank.Interfaces.EntityComponentSystem.Manager;
using Tank.Validator;

namespace Tank.Systems
{
    /// <summary>
    /// This system will damage the terrain if an event is catched
    /// </summary>
    class MapDestructionSystem : AbstractSystem
    {
        /// <summary>
        /// Create an new intance of this class
        /// </summary>
        public MapDestructionSystem() : base()
        {
            validators.Add(new MapValidator());
        }

        /// <inheritdoc/>
        public override void Initialize(IGameEngine gameEngine)
        {
            base.Initialize(gameEngine);
            eventManager.SubscribeEvent(this, typeof(DamageTerrainEvent));
        }

        /// <summary>
        /// Remove some pixels from the map component if required
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="eventArgs">The arguments from the event</param>
        public override void EventNotification(object sender, EventArgs eventArgs)
        {
            base.EventNotification(sender, eventArgs);

            if (eventArgs is DamageTerrainEvent)
            {
                DamageTerrainEvent damageTerrainEvent = (DamageTerrainEvent)eventArgs;
                List<uint> physicEntities = entityManager.GetEntitiesWithComponent<MoveableComponent>();
                foreach (uint entityId in watchedEntities)
                {
                    MapComponent map = entityManager.GetComponent<MapComponent>(entityId);

                    Circle damageCircle = damageTerrainEvent.DamageArea;
                    int xStart = damageCircle.Center.X - damageCircle.Radius;
                    int yStart = damageCircle.Center.Y - damageCircle.Radius;
                    int xEnd = xStart + damageCircle.Diameter;
                    int yEnd = yStart + damageCircle.Diameter;
                    for (int x = xStart; x < xEnd; x++)
                    {
                        for (int y = yStart; y < yEnd; y++)
                        {
                            if (damageCircle.IsInInCircle(x, y))
                            {
                                map.Map.ChangePixel(x, y, Color.Transparent, false);
                            }

                        }
                    }
                    foreach (uint physicEntity in physicEntities)
                    {
                        PlaceableComponent placeable = entityManager.GetComponent<PlaceableComponent>(physicEntity);
                        ColliderComponent collider = entityManager.GetComponent<ColliderComponent>(physicEntity);
                        if (placeable == null)
                        {
                            continue;
                        }

                        MoveableComponent moveable = entityManager.GetComponent<MoveableComponent>(physicEntity);
                        Position entityPosition = new Position(placeable.Position);
                        List<Position> positions = GetColliderPositions(entityPosition, collider);
                        foreach (Position position in positions)
                        {
                            if (damageCircle.IsInInCircle(position))
                            {
                                moveable.OnGround = false;
                                break;
                            }
                        }

                    }
                    map.Map.ApplyChanges();
                }
            }
        }

        private List<Position> GetColliderPositions(Position entityPosition, ColliderComponent collider)
        {
            List<Position> positions = new List<Position>();
            positions.Add(entityPosition);
            if (collider != null)
            {
                positions.Add(new Position(
                    entityPosition.X + collider.Collider.X + collider.Collider.Right,
                    entityPosition.Y + collider.Collider.Y
                ));
                positions.Add(new Position(
                    entityPosition.X + collider.Collider.X,
                    entityPosition.Y + collider.Collider.Y + collider.Collider.Bottom
                ));
                positions.Add(new Position(
                    entityPosition.X + collider.Collider.X + collider.Collider.Right,
                    entityPosition.Y + collider.Collider.Y + collider.Collider.Bottom
                ));
            }

            return positions;
        }
    }
}
