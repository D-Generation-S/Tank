using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Tank.Components;
using Tank.Components.Rendering;
using Tank.Components.Tags;
using TankEngine.EntityComponentSystem;

namespace Tank.Builders
{
    class FadeOutTextBuilder : BaseBuilder
    {
        private readonly SpriteFont fontToUse;

        public FadeOutTextBuilder(SpriteFont fontToUse)
        {
            this.fontToUse = fontToUse;
        }

        public override List<IComponent> BuildGameComponents(object parameter)
        {
            List<IComponent> componentList = new List<IComponent>();
            if (parameter is string text)
            {
                PlaceableComponent placeableComponent = entityManager.CreateComponent<PlaceableComponent>();
                placeableComponent.Position = Vector2.Zero;

                VisibleTextComponent visibleTextComponent = entityManager.CreateComponent<VisibleTextComponent>();
                visibleTextComponent.Text = text;
                visibleTextComponent.Font = fontToUse;
                visibleTextComponent.Color = Color.White;

                FadeComponent fadeComponent = entityManager.CreateComponent<FadeComponent>();
                fadeComponent.OpacityChange = 1f;
                fadeComponent.TargetOpacity = 0;
                fadeComponent.StartOpacity = 0;
                fadeComponent.TicksToLive = 50;

                componentList.Add(placeableComponent);
                componentList.Add(visibleTextComponent);
                componentList.Add(fadeComponent);
                componentList.Add(entityManager.CreateComponent<MessageTag>());
            }
            return componentList;
        }
    }
}
