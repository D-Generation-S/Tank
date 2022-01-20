using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Tank.Components;
using Tank.Components.GameObject;
using Tank.Components.Rendering;
using Tank.Validator;
using TankEngine.EntityComponentSystem.Components.Rendering;
using TankEngine.EntityComponentSystem.Systems;

namespace Tank.Systems
{
    internal class AnimationAttributeDisplaySystem : AbstractSystem
    {
        /// <summary>
        /// A list with all the updated data entities
        /// </summary>
        private readonly List<uint> updatedEntites;

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        public AnimationAttributeDisplaySystem() : base()
        {
            validators.Add(new AnimationAttributeDisplayValidator());
            updatedEntites = new List<uint>();
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            foreach (uint entityId in watchedEntities)
            {
                AttributeDisplayComponent attributeDisplayComponent = entityManager.GetComponent<AttributeDisplayComponent>(entityId);
                TextureComponent visibleComponent = entityManager.GetComponent<TextureComponent>(entityId);
                AttributeDisplayBaseSize size = entityManager.GetComponent<AttributeDisplayBaseSize>(entityId);
                BindComponent bindComponent = entityManager.GetComponent<BindComponent>(entityId);

                if (bindComponent == null || attributeDisplayComponent == null || visibleComponent == null || size == null)
                {
                    continue;
                }

                GameObjectData data = entityManager.GetComponent<GameObjectData>(bindComponent.BoundEntityId);
                if (data == null || !attributeDisplayComponent.MaxAttributePresent)
                {
                    continue;
                }
                string attributeToDisplay = attributeDisplayComponent.AttributeToDisplay;
                string maxAttributeToDisplay = attributeDisplayComponent.MaxAttributeName;

                if (!data.Properties.ContainsKey(attributeToDisplay) || !data.Properties.ContainsKey(maxAttributeToDisplay))
                {
                    continue;
                }
                float attributeVal = data.Properties[attributeToDisplay];
                float maxAttributeVal = data.Properties[maxAttributeToDisplay];

                float percentage = attributeVal / maxAttributeVal;
                int leftWidth = (int)(size.BaseSize.Width * percentage);
                int cutoff = size.BaseSize.Width - leftWidth;
                Rectangle newSize = new Rectangle(size.BaseSize.X, size.BaseSize.Y, size.BaseSize.Width - cutoff, size.BaseSize.Height);
                visibleComponent.Source = newSize;

                if (!updatedEntites.Contains(bindComponent.BoundEntityId))
                {
                    updatedEntites.Add(bindComponent.BoundEntityId);
                }
            }
        }

        /// <inheritdoc/>
        public override void LateUpdate()
        {
            base.LateUpdate();
            foreach (uint updatedEntity in updatedEntites)
            {
                GameObjectData data = entityManager.GetComponent<GameObjectData>(updatedEntity);
                if (data != null)
                {
                    data.DataChanged = false;
                }
            }

            updatedEntites.Clear();
        }
    }
}
