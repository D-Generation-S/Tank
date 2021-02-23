using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Tank.Components;
using Tank.Components.Forces;
using Tank.Interfaces.Builders;
using Tank.Interfaces.EntityComponentSystem;
using Tank.Interfaces.Factories;

namespace Tank.Builders
{
    /// <summary>
    /// This class will build you an explosion based on a sprite sheet and some animation frames
    /// </summary>
    class BaseExplosionBuilder : IGameObjectBuilder
    {
        /// <summary>
        /// The spritesheet containing the frames
        /// </summary>
        private readonly Texture2D spriteSheet;

        /// <summary>
        /// A factory to create sounds for this effect
        /// </summary>
        private readonly ISoundFactory soundFactory;

        /// <summary>
        /// The list with all the animations frames to use
        /// </summary>
        private readonly List<Rectangle> animationFrames;

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="spriteSheet">The sprite sheet to use</param>
        /// <param name="animationFrames">All the animation frames available in the sprite sheet</param>
        public BaseExplosionBuilder(Texture2D spriteSheet, List<Rectangle> animationFrames)
            : this(spriteSheet, animationFrames, null)
        {
        }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="spriteSheet">The sprite sheet to use</param>
        /// <param name="animationFrames">All the animation frames available in the sprite sheet</param>
        public BaseExplosionBuilder(Texture2D spriteSheet, List<Rectangle> animationFrames, ISoundFactory soundFactory)
        {
            this.spriteSheet = spriteSheet;
            this.animationFrames = animationFrames;
            this.soundFactory = soundFactory;
        }

        /// <summary>
        /// Create the game components for the new entity
        /// </summary>
        /// <returns>A component list making up the new explosion entity</returns>
        public List<IComponent> BuildGameComponents()
        {
            List<IComponent> components = new List<IComponent>();
            PlaceableComponent placeableComponent = new PlaceableComponent();
            VisibleComponent visibleComponent = new VisibleComponent();
            visibleComponent.Texture = spriteSheet;
            visibleComponent.Source = animationFrames[0];
            visibleComponent.Destination = animationFrames[0];
            placeableComponent.Position = new Vector2(-(animationFrames[0].Width / 2), -(animationFrames[0].Height / 2));
            AnimationComponent animation = new AnimationComponent(0.03f, animationFrames);
            animation.Name = "Idle";

            if (soundFactory != null)
            {
                SoundEffectComponent soundEffect = new SoundEffectComponent(soundFactory.GetRandomSoundEffect(), true);
                components.Add(soundEffect);
            }

            float radius = animationFrames[0].Width + animationFrames[0].Height;
            radius /= 2;
            ForceComponent forceComponent = new ForceComponent(radius, 100, Enums.ForceTypeEnum.Push, Enums.ForceTriggerTimeEnum.Add);

            components.Add(placeableComponent);
            components.Add(visibleComponent);
            components.Add(animation);
            components.Add(forceComponent);

            return components;
        }
    }
}
