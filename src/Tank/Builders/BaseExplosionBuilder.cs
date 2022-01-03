using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Tank.Components;
using Tank.Components.Forces;
using Tank.Components.Rendering;
using TankEngine.EntityComponentSystem;
using TankEngine.Factories;
using TankEngine.Randomizer;

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
        private readonly IFactory<SoundEffect> soundFactory;

        /// <summary>
        /// The list with all the animations frames to use
        /// </summary>
        private readonly List<Rectangle> animationFrames;

        /// <summary>
        /// Randomizer to use for rotation
        /// </summary>
        private readonly IRandomizer randomizer;

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
        /// The center of rotation
        /// </summary>
        private Vector2 rotationCenter;

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="spriteSheet">The sprite sheet to use</param>
        /// <param name="animationFrames">All the animation frames available in the sprite sheet</param>
        public BaseExplosionBuilder(
            Texture2D spriteSheet,
            List<Rectangle> animationFrames,
            IRandomizer randomizer
            )
            : this(spriteSheet, animationFrames, randomizer, null)
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
            IRandomizer randomizer,
            IFactory<SoundEffect> soundFactory
            )
        {
            this.spriteSheet = spriteSheet;
            this.animationFrames = animationFrames;
            this.randomizer = randomizer;
            this.soundFactory = soundFactory;
            position = new Vector2(-(animationFrames[0].Width / 2), -(animationFrames[0].Height / 2));
            name = "Idle";
            radius = animationFrames[0].Width + animationFrames[0].Height;
            rotationCenter = new Vector2(animationFrames[0].Width / 2, animationFrames[0].Height / 2);
        }

        /// <summary>
        /// Create the game components for the new entity
        /// </summary>
        /// <returns>A component list making up the new explosion entity</returns>
        public override List<IComponent> BuildGameComponents(object parameter)
        {
            List<IComponent> returnComponents = new List<IComponent>();
            if (entityManager == null)
            {
                return returnComponents;
            }

            float rotation = randomizer.GetNewNumber(0, 360);


            VisibleComponent visibleComponent = entityManager.CreateComponent<VisibleComponent>();
            visibleComponent.Texture = spriteSheet;
            visibleComponent.Source = animationFrames[0];
            visibleComponent.Destination = animationFrames[0];
            visibleComponent.SingleTextureSize = animationFrames[0];
            visibleComponent.RotationCenter = rotationCenter;

            PlaceableComponent placeableComponent = entityManager.CreateComponent<PlaceableComponent>();
            placeableComponent.Position = position;
            placeableComponent.Rotation = MathHelper.ToRadians(rotation);

            AnimationComponent animation = entityManager.CreateComponent<AnimationComponent>();
            animation.FrameSeconds = 0.03f;
            animation.SpriteSources = animationFrames;
            animation.Name = name;

            if (soundFactory != null)
            {
                SoundEffectComponent soundEffect = entityManager.CreateComponent<SoundEffectComponent>();
                soundEffect.SoundEffect = soundFactory.GetNewObject();
                soundEffect.RandomPitch = true;
                returnComponents.Add(soundEffect);
            }


            ForceComponent forceComponent = entityManager.CreateComponent<ForceComponent>();
            forceComponent.ForceRadius = radius;
            forceComponent.ForceBaseStrenght = 50;
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