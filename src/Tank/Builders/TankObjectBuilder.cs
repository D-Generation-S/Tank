using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Tank.Components;
using Tank.Components.Rendering;
using Tank.Components.Tags;
using Tank.DataStructure;
using Tank.Interfaces.EntityComponentSystem;

namespace Tank.Builders
{
    /// <summary>
    /// Create new tank object
    /// </summary>
    class TankObjectBuilder : BaseBuilder
    {
        /// <summary>
        /// The start position of the tank
        /// </summary>
        private readonly Vector2 startPosition;

        /// <summary>
        /// The spritesheet to use
        /// </summary>
        private readonly Texture2D spriteSheet;

        /// <summary>
        /// The animation frames
        /// </summary>
        private readonly List<Rectangle> animationFrames;
        private readonly Rectangle colliderDestination;

        /// <summary>
        /// The shader effect to use
        /// </summary>
        private readonly Effect effect;

        /// <summary>
        /// Create a new instance of this class to spawn game objects
        /// </summary>
        /// <param name="spriteSheet"></param>
        /// <param name="animationFrames"></param>
        public TankObjectBuilder(Vector2 startPosition, Texture2D spriteSheet, List<Rectangle> animationFrames)
            : this(startPosition, spriteSheet, animationFrames, null)
        {
        }

        /// <summary>
        /// Create a new instance of this class to spawn game objects
        /// </summary>
        /// <param name="spriteSheet"></param>
        /// <param name="animationFrames"></param>
        public TankObjectBuilder(Vector2 startPosition, Texture2D spriteSheet, List<Rectangle> animationFrames, Effect effect)
        {
            this.startPosition = startPosition;
            this.spriteSheet = spriteSheet;
            this.animationFrames = animationFrames;
            this.effect = effect;
            int textureWidth = animationFrames[0].Width;
            int textureHeight = animationFrames[0].Height;
            colliderDestination = new Rectangle(-textureWidth / 2, -textureHeight / 2, textureWidth, textureHeight);
        }

        /// <summary>
        /// Build all the game components
        /// </summary>
        /// <returns></returns>
        public override List<IComponent> BuildGameComponents(object parameter)
        {
            List<IComponent> returnComponents = new List<IComponent>();
            if (entityManager == null)
            {
                return returnComponents;
            }
            PlaceableComponent placeableComponent = entityManager.CreateComponent<PlaceableComponent>();
            placeableComponent.Position = startPosition + new Vector2(spriteSheet.Width, spriteSheet.Height) * -1;
            VisibleComponent visibleComponent = entityManager.CreateComponent<VisibleComponent>();
            visibleComponent.Color = Color.White;
            visibleComponent.Source = animationFrames[0];
            visibleComponent.Destination = animationFrames[0];
            visibleComponent.Texture = spriteSheet;
            visibleComponent.ShaderEffect = effect;
            visibleComponent.DrawMiddle = true;
            MoveableComponent moveable = entityManager.CreateComponent<MoveableComponent>();
            moveable.Mass = 15;
            moveable.ApplyPhysic = true;
            ColliderComponent collider = entityManager.CreateComponent<ColliderComponent>();
            collider.Collider = colliderDestination;

            PlayerControllableComponent controllableComponent = entityManager.CreateComponent<PlayerControllableComponent>();
            controllableComponent.Controller = new StaticKeyboardControls();
            GameObjectTag gameObjectTag = entityManager.CreateComponent<GameObjectTag>();

            returnComponents.Add(placeableComponent);
            returnComponents.Add(visibleComponent);
            returnComponents.Add(moveable);
            returnComponents.Add(collider);
            returnComponents.Add(controllableComponent);
            returnComponents.Add(gameObjectTag);

            return returnComponents;
        }
    }
}
