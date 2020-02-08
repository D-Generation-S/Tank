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
using Tank.Interfaces.Container;
using Tank.Interfaces.Entity;
using Tank.Interfaces.Implementations;
using Tank.Interfaces.System;

namespace Tank.Code.Systems.GameManager
{ 
    class DefaultEntitySystem : BaseSystem, IEntitySystem
    {
        private readonly List<IEntity> entities;
        public IList<IEntity> Entities => entities;

        public DefaultEntitySystem()
        {
            entities = new List<IEntity>();
            Initzialize(Guid.NewGuid().ToString());
        }

        public override void Initzialize(string uniqueName)
        {
            base.Initzialize(uniqueName);
            active = true;
            alive = true;
        }

        public override string AddEntity(IEntity entity)
        {
            string name = Guid.NewGuid().ToString();
            entity.Initzialize(name);

            if (entity is ISystem)
            {
                ((ISystem)entity).SetParent(this);
            }

            int entityCount = entities.Count;
            for (int i = 0; i < entityCount; i++)
            {
                if (entities[i] is ISystem)
                {
                    ((ISystem)entities[i]).AddEntity(entity);
                }
            }

            entities.Add(entity);

            return name;
        }

        public override bool RemoveEntity(string entityName)
        {
            int counter = entities.RemoveAll(entity => entity.UniqueName == entityName);

            bool removingOkay = true;
            foreach (IEntity systemEntity in entities)
            {
                if (systemEntity is ISystem)
                {
                    removingOkay &= ((ISystem)systemEntity).RemoveEntity(entityName);
                }
            }

            return counter > 0 && removingOkay;
        }

        public void Draw(GameTime gameTime)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                if (entities[i] is IDrawableEntity && entities[i].Active)
                {
                    ((IDrawableEntity)entities[i]).Draw(gameTime);
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            CleanEntities();
            for (int i = 0; i < entities.Count; i++)
            {
                if (entities[i].Active)
                {
                    entities[i].Update(gameTime);
                }
            }
        }

        private void CleanEntities()
        {
            for (int i = entities.Count - 1; i > 0; i--)
            {
                if (!entities[i].Alive)
                {
                    RemoveEntity(entities[i].UniqueName);
                }
            }
        }

        public override void RecieveMessage(ISystemMessage message)
        {
            if (message.Target == UniqueName)
            {
                PerformeMessageAction(message);
                return;
            }

            SendMessage(message);
        }

        protected override void SendMessage(ISystemMessage message)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                if (entities[i] is ISystem)
                {
                    ((ISystem)entities[i]).RecieveMessage(message);
                }
            }
        }

        private void PerformeMessageAction(ISystemMessage message)
        {
            object data = message.Data;
            if (data is IEntity)
            {
                string name = AddEntity((IEntity)data);
                ISystemMessage newMessage = new DefaultSystemMessage(
                    UniqueName,
                    message.Sender,
                    name
                );
                SendMessage(newMessage);
            }
        }
    }
}
