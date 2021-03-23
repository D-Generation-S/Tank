using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Tank.Components;
using Tank.Components.GameObject;
using Tank.Components.Rendering;
using Tank.Components.Tags;
using Tank.Interfaces.EntityComponentSystem;

namespace Tank.Builders
{
    /// <summary>
    /// Create new tank object
    /// </summary>
    class TankObjectBuilder : BaseBuilder
    {

        /// <summary>
        /// The spritesheet to use
        /// </summary>
        private readonly Texture2D spriteSheet;

        /// <summary>
        /// The animation frames
        /// </summary>
        private readonly List<Rectangle> animationFrames;

        /// <summary>
        /// The collidert destination
        /// </summary>
        private readonly Rectangle colliderDestination;

        /// <summary>
        /// The shader effect to use
        /// </summary>
        private readonly Effect effect;

        /// <summary>
        /// The center or rotation
        /// </summary>
        private Vector2 RotationCenter;

        /// <summary>
        /// Create a new instance of this class to spawn game objects
        /// </summary>
        /// <param name="spriteSheet"></param>
        /// <param name="animationFrames"></param>
        public TankObjectBuilder(Texture2D spriteSheet, List<Rectangle> animationFrames)
            : this(spriteSheet, animationFrames, null)
        {
        }

        /// <summary>
        /// Create a new instance of this class to spawn game objects
        /// </summary>
        /// <param name="spriteSheet"></param>
        /// <param name="animationFrames"></param>
        public TankObjectBuilder(Texture2D spriteSheet, List<Rectangle> animationFrames, Effect effect)
        {
            this.spriteSheet = spriteSheet;
            this.animationFrames = animationFrames;
            this.effect = effect;
            int textureWidth = animationFrames[0].Width;
            int textureHeight = animationFrames[0].Height;
            colliderDestination = new Rectangle(-textureWidth / 2, -textureHeight / 2, textureWidth, textureHeight);
            RotationCenter = new Vector2(textureWidth / 2, textureHeight / 2);
        }

        /// <summary>
        /// Build all the game components
        /// </summary>
        /// <returns></returns>
        public override List<IComponent> BuildGameComponents(object parameter)
        {
            if (entityManager == null)
            {
                return new List<IComponent>();
            }

            if (parameter is Vector2 startPosition)
            {
                return CreateComponents(startPosition);
            }

            return new List<IComponent>();
        }

        protected List<IComponent> CreateComponents(Vector2 startPosition)
        {
            List<IComponent> returnComponents = new List<IComponent>();
            GameObjectData gameObjectData = entityManager.CreateComponent<GameObjectData>();
            gameObjectData.Properties.Add("Health", 100f);
            gameObjectData.Properties.Add("MaxHealth", gameObjectData.Properties["Health"]);
            gameObjectData.Properties.Add("Armor", 10f);
            gameObjectData.Properties.Add("Accuracy", 1f);
            gameObjectData.DataChanged = true;
            PlaceableComponent placeableComponent = entityManager.CreateComponent<PlaceableComponent>();
            placeableComponent.Position = startPosition + new Vector2(spriteSheet.Width, spriteSheet.Height) * -1;
            VisibleComponent visibleComponent = entityManager.CreateComponent<VisibleComponent>();
            visibleComponent.Color = Color.White;
            visibleComponent.Source = animationFrames[0];
            visibleComponent.Destination = animationFrames[0];
            visibleComponent.Texture = spriteSheet;
            visibleComponent.ShaderEffect = effect;
            visibleComponent.RotationCenter = RotationCenter;
            MoveableComponent moveable = entityManager.CreateComponent<MoveableComponent>();
            moveable.Mass = 15;
            moveable.ApplyPhysic = true;
            ColliderComponent collider = entityManager.CreateComponent<ColliderComponent>();
            collider.Collider = colliderDestination;

            //PlayerControllableComponent controllableComponent = entityManager.CreateComponent<PlayerControllableComponent>();
            //controllableComponent.Controller = new StaticKeyboardControls();
            GameObjectTag gameObjectTag = entityManager.CreateComponent<GameObjectTag>();

            returnComponents.Add(placeableComponent);
            returnComponents.Add(gameObjectData);
            returnComponents.Add(visibleComponent);
            returnComponents.Add(moveable);
            returnComponents.Add(collider);
            //returnComponents.Add(controllableComponent);
            returnComponents.Add(gameObjectTag);
            returnComponents.Add(entityManager.CreateComponent<PlayerControlledTag>());
            returnComponents.Add(entityManager.CreateComponent<ControllableGameObject>());

            return returnComponents;
        }
    }
}
