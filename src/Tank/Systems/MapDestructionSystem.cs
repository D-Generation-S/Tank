using Microsoft.Xna.Framework;
using System;
using Tank.Components;
using Tank.DataStructure.Geometrics;
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
        private MapComponent map;
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

        /// <inheritdoc/>
        protected override void AddEntity(uint entityId)
        {
            base.AddEntity(entityId);
            if (entityManager.HasComponent(entityId, typeof(MapComponent)))
            {
                map = entityManager.GetComponent<MapComponent>(entityId);
            }
        }

        /// <summary>
        /// Remove some pixels from the map component if required
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="eventArgs">The arguments from the event</param>
        public override void EventNotification(object sender, EventArgs eventArgs)
        {
            base.EventNotification(sender, eventArgs);

            if (map != null && eventArgs is DamageTerrainEvent damageTerrainEvent)
            {
                Circle damageCircle = damageTerrainEvent.DamageArea;
                Vector2 start = damageCircle.Center - Vector2.One * damageCircle.Radius;
                Vector2 end = start + Vector2.One * damageCircle.Diameter;
                for (int x = (int)start.X; x < (int)end.X; x++)
                {
                    for (int y = (int)start.Y; y < (int)end.Y; y++)
                    {
                        if (damageCircle.IsInInCircle(x, y) && x >= 0 && x < map.Map.Width)
                        {
                            map.Map.ChangePixel(x, y, Color.Transparent, false);
                        }
                    }
                }
            }
        }

        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (map == null || map.Map == null)
            {
                return;
            }
            map.Map.ApplyChanges();
        }
    }
}
