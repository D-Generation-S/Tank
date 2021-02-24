using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Tank.Components;
using Tank.Components.Forces;
using Tank.Components.Rendering;
using Tank.Interfaces.Builders;
using Tank.Interfaces.EntityComponentSystem;
using Tank.Interfaces.Factories;

namespace Tank.Builders
{
    /// <summary>
    /// This class will build you an explosion based on a sprite sheet and some animation frames
    /// </summary>
    class BaseExplosionBuilder : BaseBuilder
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
        /// The position to use
        /// </summary>
        private readonly Vector2 position;

        /// <summary>
        /// The name for the animation
        /// </summary>
        private readonly string name;

        /// <summary>
        /// The damage radius
        /// </summary>
        private readonly int radius;

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="spriteSheet">The sprite sheet to use</param>
        /// <param name="animationFrames">All the animation frames available in the sprite sheet</param>
        public BaseExplosionBuilder(
            Texture2D spriteSheet,
            List<Rectangle> animationFrames
            )
            : this(spriteSheet, animationFrames, null)
        {
        }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="spriteSheet">The sprite sheet to use</param>
        /// <param name="animationFrames">All the animation frames available in the sprite sheet</param>
        public BaseExplosionBuilder(
            Texture2D spriteSheet,
            List<Rectangle> animationFrames,
            ISoundFactory soundFactory
            )
        {
            this.spriteSheet = spriteSheet;
            this.animationFrames = animationFrames;
            this.soundFactory = soundFactory;
            position = new Vector2(-(animationFrames[0].Width / 2), -(animationFrames[0].Height / 2));
            name = "Idle";
            radius = animationFrames[0].Width + animationFrames[0].Height;
            radius /= 2;
        }

        /// <summary>
        /// Create the game components for the new entity
        /// </summary>
        /// <returns>A component list making up the new explosion entity</returns>
        public override List<IComponent> BuildGameComponents()
        {
            List<IComponent> returnComponents = new List<IComponent>();
            if (entityManager == null)
            {
                return returnComponents;
            }
            PlaceableComponent placeableComponent = entityManager.CreateComponent<PlaceableComponent>();
            VisibleComponent visibleComponent = entityManager.CreateComponent<VisibleComponent>();
            visibleComponent.Texture = spriteSheet;
            visibleComponent.Source = animationFrames[0];
            visibleComponent.Destination = animationFrames[0];
            placeableComponent.Position = position;
            AnimationComponent animation = entityManager.CreateComponent<AnimationComponent>();
            animation.FrameSeconds = 0.03f;
            animation.SpriteSources = animationFrames;
            animation.Name = name;

            if (soundFactory != null)
            {

                SoundEffectComponent soundEffect = entityManager.CreateComponent<SoundEffectComponent>();
                soundEffect.SoundEffect = soundFactory.GetRandomSoundEffect();
                soundEffect.RandomPitch = true;
                returnComponents.Add(soundEffect);
            }


            ForceComponent forceComponent = entityManager.CreateComponent<ForceComponent>();
            forceComponent.ForceRadius = radius;
            forceComponent.ForceBaseStrenght = 100;
            forceComponent.ForceType = Enums.ForceTypeEnum.Push;
            forceComponent.ForceTrigger = Enums.ForceTriggerTimeEnum.Add;

            returnComponents.Add(placeableComponent);
            returnComponents.Add(visibleComponent);
            returnComponents.Add(animation);
            returnComponents.Add(forceComponent);

            return returnComponents;
        }
    }
}
