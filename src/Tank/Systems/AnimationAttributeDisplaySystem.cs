using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Tank.Components;
using Tank.Components.GameObject;
using Tank.Components.Rendering;
using Tank.Validator;

namespace Tank.Systems
{
    internal class AnimationAttributeDisplaySystem : AbstractSystem
    {
        public AnimationAttributeDisplaySystem() : base()
        {
            validators.Add(new AnimationAttributeDisplayValidator());
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            foreach(uint entityId in watchedEntities)
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
                data.DataChanged = false;
            }
        }
    }
}
