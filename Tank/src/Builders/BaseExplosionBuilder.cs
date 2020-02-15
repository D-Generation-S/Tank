using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tank.src.Components;
using Tank.src.Interfaces.Builders;
using Tank.src.Interfaces.EntityComponentSystem;

namespace Tank.src.Builders
{
    class BaseExplosionBuilder : IGameObjectBuilder
    {
        private readonly Texture2D spriteSheet;
        private readonly List<Rectangle> animationFrames;

        public BaseExplosionBuilder(Texture2D spriteSheet, List<Rectangle> animationFrames)
        {
            this.spriteSheet = spriteSheet;
            this.animationFrames = animationFrames;
        }

        public List<IComponent> BuildGameComponents()
        {
            List<IComponent> components = new List<IComponent>();
            PlaceableComponent placeableComponent = new PlaceableComponent();
            VisibleComponent visibleComponent = new VisibleComponent();
            visibleComponent.Texture = spriteSheet;
            visibleComponent.Source = animationFrames[0];
            visibleComponent.Destination = animationFrames[0];
            placeableComponent.Position = new Vector2(animationFrames[0].Width / 2, animationFrames[0].Height / 2);
            AnimationComponent animation = new AnimationComponent(0.1f, animationFrames);
            animation.Name = "Idle";

            components.Add(placeableComponent);
            components.Add(visibleComponent);
            components.Add(animation);

            return components;
        }
    }
}
