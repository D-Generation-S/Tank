using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Tank.Components;
using Tank.Components.GameObject;
using Tank.Components.Rendering;
using Tank.Validator;
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

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            foreach (uint entityId in watchedEntities)
            {
                AttributeDisplayComponent attributeDisplayComponent = entityManager.GetComponent<AttributeDisplayComponent>(entityId);
                VisibleComponent visibleComponent = entityManager.GetComponent<VisibleComponent>(entityId);
                BindComponent bindComponent = entityManager.GetComponent<BindComponent>(entityId);

                if (bindComponent == null || attributeDisplayComponent == null || visibleComponent == null)
                {
                    continue;
                }

                GameObjectData data = entityManager.GetComponent<GameObjectData>(bindComponent.BoundEntityId);
                if (data == null || !data.DataChanged || !attributeDisplayComponent.MaxAttributePresent)
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
                int leftWidth = (int)(visibleComponent.SingleTextureSize.Width * percentage);
                int cutoff = visibleComponent.SingleTextureSize.Width - leftWidth;

                visibleComponent.CutoffRight = cutoff;
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
