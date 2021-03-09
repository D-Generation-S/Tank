using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Tank.Components;
using Tank.Components.Rendering;
using Tank.Components.Tags;
using Tank.DataStructure.Spritesheet;
using Tank.Interfaces.EntityComponentSystem;

namespace Tank.Builders
{
    class CloudBuilder : BaseBuilder
    {
        private readonly Texture2D texture;
        private readonly List<Rectangle> animationFrames;

        public CloudBuilder(Texture2D texture, List<Rectangle> animationFrames)
        {
            this.texture = texture;
            this.animationFrames = animationFrames;
        }

        /// <inheritdoc>/>
        public override List<IComponent> BuildGameComponents(object parameter)
        {
            List<IComponent> returnList = new List<IComponent>();
            if (entityManager == null)
            {
                return returnList;
            }
            PlaceableComponent placeableComponent = entityManager.CreateComponent<PlaceableComponent>();
            MoveableComponent moveableComponent = entityManager.CreateComponent<MoveableComponent>();
            moveableComponent.Velocity = Vector2.UnitX;
            VisibleComponent visibleComponent = entityManager.CreateComponent<VisibleComponent>();
            visibleComponent.Texture = texture;
            visibleComponent.Destination = animationFrames[0];
            visibleComponent.Source = animationFrames[0];
            visibleComponent.SingleTextureSize = animationFrames[0];
            visibleComponent.DrawMiddle = true;
            AnimationComponent animationComponent = entityManager.CreateComponent<AnimationComponent>();
            animationComponent.Active = true;
            animationComponent.Name = "Idle";
            animationComponent.Loop = true;
            animationComponent.SpriteSources = animationFrames;

            returnList.Add(visibleComponent);
            returnList.Add(new MoveableOnlyTag());
            //returnList.Add(animationComponent);
            returnList.Add(placeableComponent);
            returnList.Add(moveableComponent);
            return returnList;
        }
    }
}
