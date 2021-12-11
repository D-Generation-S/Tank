using Microsoft.Xna.Framework;
using System.Linq;
using Tank.Components;
using Tank.Components.Rendering;
using Tank.Components.Tags;
using Tank.DataStructure.Geometrics;
using Tank.Events;
using Tank.Events.PhysicBased;
using Tank.Events.TerrainEvents;
using Tank.Interfaces.EntityComponentSystem.Manager;
using Tank.Validator;

namespace Tank.Systems
{
    /// <summary>
    /// This system will damage the terrain if an event is catched
    /// </summary>
    class MapSystem : AbstractSystem
    {
        //private MapComponent map;
        /// <summary>
        /// Create an new intance of this class
        /// </summary>
        public MapSystem() : base()
        {
            validators.Add(new MapValidator());
        }

        /// <inheritdoc/>
        public override void Initialize(IGameEngine gameEngine)
        {
            base.Initialize(gameEngine);
            eventManager.SubscribeEvent(this, typeof(DamageTerrainEvent));
            eventManager.SubscribeEvent(this, typeof(MapCollisionEvent));
        }

        /// <inheritdoc/>
        protected override void AddEntity(uint entityId)
        {
            base.AddEntity(entityId);
            /**
            if (entityManager.HasComponent(entityId, typeof(MapComponent)))
            {
                map = entityManager.GetComponent<MapComponent>(entityId);
            }
            **/
        }

        /// <summary>
        /// Remove some pixels from the map component if required
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="eventArgs">The arguments from the event</param>
        public override void EventNotification(object sender, IGameEvent eventArgs)
        {
            base.EventNotification(sender, eventArgs);
            MapComponent map = GetMapComponent();
            if (map == null)
            {
                return;
            }

            if (eventArgs is DamageTerrainEvent damageTerrainEvent)
            {
                Circle damageCircle = damageTerrainEvent.DamageArea;
                Vector2 start = damageCircle.Center - Vector2.One * damageCircle.Radius;
                Vector2 end = start + Vector2.One * damageCircle.Diameter;
                for (int x = (int)start.X; x < (int)end.X; x++)
                {
                    for (int y = (int)start.Y; y < (int)end.Y; y++)
                    {
                        if (damageCircle.IsInInCircle(x, y) && x >= 0 && x < map.Width)
                        {
                            map.ChangedImageData.SetValue(x, y, Color.Transparent);
                            map.RenderRequired = true;
                        }
                    }
                }
            }

            if (eventArgs is MapCollisionEvent mapCollision)
            {
                uint entityId = mapCollision.EntityId;
                if (!entityManager.HasComponent<AddTerrainTag>(entityId))
                {
                    return;
                }

                ColliderComponent collider = entityManager.GetComponent<ColliderComponent>(entityId);
                VisibleComponent visual = entityManager.GetComponent<VisibleComponent>(entityId);

                if (collider == null || visual == null)
                {
                    return;
                }

                Vector2 start = new Vector2(-collider.Collider.Bottom, -collider.Collider.Height);
                start += mapCollision.Position;
                Vector2 end = start + collider.Collider.Size.ToVector2();

                for (int x = (int)start.X; x < (int)end.X; x++)
                {
                    for (int y = (int)start.Y; y < (int)end.Y; y++)
                    {
                        if (!map.Map.IsPixelSolid(x, y) && x >= 0 && x < map.Map.Width)
                        {
                            map.Map.ChangePixel(x, y, visual.Color, false);
                        }
                    }
                }

                entityManager.RemoveEntity(entityId);
            }
        }

        /// <summary>
        /// Get the map component
        /// </summary>
        /// <returns>Get the map component if present</returns>
        private MapComponent GetMapComponent()
        {
            return entityManager.GetEntitiesWithComponent<MapComponent>()
                                .Select(id => entityManager.GetComponent<MapComponent>(id))
                                .FirstOrDefault();
        }

        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            MapComponent map = GetMapComponent();
            if (map == null || !map.RenderRequired)
            {
                return;
            }
            map.ImageData = map.ChangedImageData;
            
            VisibleComponent visible = entityManager.GetEntitiesWithComponent<MapComponent>()
                                                    .Select(id => entityManager.GetComponent<VisibleComponent>(id))
                                                    .FirstOrDefault();
            if (visible == null)
            {
                return;
            }

            map.RenderRequired = false;
            visible.Texture.SetData<Color>(map.ChangedImageData.Array);

        }
    }
}
