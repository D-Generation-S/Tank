using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tank.src.Components;
using Tank.src.DataStructure;
using Tank.src.Events.TerrainEvents;
using Tank.src.Interfaces.EntityComponentSystem.Manager;
using Tank.src.Validator;

namespace Tank.src.Systems
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
                    map.Map.ApplyChanges();
                }
            }

        }
    }
}
