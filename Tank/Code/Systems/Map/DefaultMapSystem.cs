using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tank.Code.BaseClasses.Entites;
using Tank.Code.BaseClasses.Systems;
using Tank.Code.DataContainer;
using Tank.Code.DataContainer.Messages;
using Tank.Code.Entities;
using Tank.Code.Implementations;
using Tank.Interfaces.Container;
using Tank.Interfaces.Entity;
using Tank.Interfaces.Implementations;
using Tank.Interfaces.MapGenerators;
using Tank.Interfaces.System;

namespace Tank.Code.Systems.Map
{
    class DefaultMapSystem : BaseSystem, IMapSystem
    {
        private readonly List<ICollideableEntity> entites;

        private IMap currentMap;
        private string mapName;

        private bool waitingForId;

        public DefaultMapSystem()
        {
            entites = new List<ICollideableEntity>();
        }

        public override void Initzialize(string uniqueName)
        {
            active = true;
            alive = true;
            waitingForId = false;
            mapName = String.Empty;
            base.Initzialize(uniqueName);
        }

        public override string AddEntity(IEntity entity)
        {
            if (entity is IMap)
            {
                currentMap = (IMap)entity;
                waitingForId = true;

                IRenderer renderer = new SpriteRenderer();
                renderer.SetTexture(currentMap.Image);
                renderer.Position = new Vector2(0, 0);
                renderer.Size = new Vector2(currentMap.Width, currentMap.Height);
                IEntity mapDrawer = new DrawableEntity(renderer);

                ISystemMessage message = new DefaultSystemMessage(
                    UniqueName,
                    parent.UniqueName,
                    mapDrawer
                );
                waitingForId = true;
                SendMessage(message);
                return entity.UniqueName;
            }

            if (entity is ICollideableEntity)
            {
                entites.Add((ICollideableEntity)entity);
                return entity.UniqueName;
            }

            return String.Empty;
        }

        public override bool RemoveEntity(string entityName)
        {
            if (currentMap != null && entityName == currentMap.UniqueName)
            {
                currentMap = null;
                return true;
            }

            int counter = entites.RemoveAll(entity => entity.UniqueName == entityName);

            return counter > 0;
        }

        public override void Update(GameTime gameTime)
        {
           if (currentMap == null)
           {
                return;
           }

            int entityCount = entites.Count;
            for (int i = 0; i < entityCount; i++)
            {
                if (!entites[i].MapCollision)
                {
                    continue;
                }
                Rectangle collider = entites[i].Collider;

                Position positionToCheck = new Position(collider.X + collider.Width, collider.Y + collider.Height);
                if (currentMap.CollissionMap.GetValue(positionToCheck.X, positionToCheck.Y))
                {
                    if (entites[i] is IPhysicEntity)
                    {
                        ((IPhysicEntity)entites[i]).OnGround = true;
                        for (int y = 0; y < collider.Height; y++)
                        {
                            if (!currentMap.CollissionMap.GetValue(positionToCheck.X, positionToCheck.Y - y))
                            {
                                int newYPosition = positionToCheck.Y - y;
                                newYPosition -= entites[i].Collider.Height;
                                entites[i].Position = new Vector2(entites[i].Position.X, newYPosition);
                                break;
                            }
                        }
                    }


                    
                }

            }
        }

        public override void RecieveMessage(ISystemMessage message)
        {
            if (waitingForId && message.Sender == parent.UniqueName)
            {
                waitingForId = false;
                mapName = message.Data.ToString();
            }
        }
    }
}
