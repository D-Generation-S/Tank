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
    internal class TextAttributeDisplaySystem : AbstractSystem
    {
        public TextAttributeDisplaySystem() : base()
        {
            validators.Add(new TextAttributeDisplayValidator());
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            foreach(uint entityId in watchedEntities)
            {
                AttributeDisplayComponent attributeDisplayComponent = entityManager.GetComponent<AttributeDisplayComponent>(entityId);
                VisibleTextComponent textComponent = entityManager.GetComponent<VisibleTextComponent>(entityId);
                BindComponent bindComponent = entityManager.GetComponent<BindComponent>(entityId);

                if (bindComponent == null || attributeDisplayComponent == null || textComponent == null)
                {
                    continue;
                }

                GameObjectData data = entityManager.GetComponent<GameObjectData>(bindComponent.BoundEntityId);
                if (data == null || !data.DataChanged)
                {
                    continue;
                }
                string textToShow = string.Empty;
                string attributeToDisplay = attributeDisplayComponent.AttributeToDisplay;

                if (data.Properties.ContainsKey(attributeToDisplay))
                {
                    textToShow += Math.Round(data.Properties[attributeToDisplay]);
                }

                if (attributeDisplayComponent.MaxAttributePresent)
                {
                    string maxAttributeName = attributeDisplayComponent.MaxAttributeName;
                    if (data.Properties.ContainsKey(maxAttributeName))
                    {
                        textToShow += "/";
                        textToShow += data.Properties[maxAttributeName];
                    }
                }


                Vector2 offset = bindComponent.Offset;
                offset.X = -textComponent.Font.MeasureString(textToShow).X / 2;
                bindComponent.Offset = offset;
                textComponent.Text = textToShow;
                data.DataChanged = false;
            }
        }
    }
}
