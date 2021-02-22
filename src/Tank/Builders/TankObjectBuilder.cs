using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Tank.Components;
using Tank.Components.Tags;
using Tank.DataStructure;
using Tank.Interfaces.Builders;
using Tank.Interfaces.EntityComponentSystem;

namespace Tank.Builders
{
    /// <summary>
    /// Create new tank object
    /// </summary>
    class TankObjectBuilder : IGameObjectBuilder
    {
        /// <summary>
        /// The start position of the tank
        /// </summary>
        private readonly Position startPosition;

        /// <summary>
        /// The spritesheet to use
        /// </summary>
        private readonly Texture2D spriteSheet;

        /// <summary>
        /// The animation frames
        /// </summary>
        private readonly List<Rectangle> animationFrames;

        /// <summary>
        /// Create a new instance of this class to spawn game objects
        /// </summary>
        /// <param name="spriteSheet"></param>
        /// <param name="animationFrames"></param>
        public TankObjectBuilder(Position startPosition, Texture2D spriteSheet, List<Rectangle> animationFrames)
        {
            this.startPosition = startPosition;
            this.spriteSheet = spriteSheet;
            this.animationFrames = animationFrames;
        }

        /// <summary>
        /// Build all the game components
        /// </summary>
        /// <returns></returns>
        public List<IComponent> BuildGameComponents()
        {
            List<IComponent> returnComponents = new List<IComponent>();
            PlaceableComponent placeableComponent = new PlaceableComponent()
            {
                Position = startPosition.GetVector2() + new Vector2(spriteSheet.Width, spriteSheet.Height) * -1
            };
            VisibleComponent visibleComponent = new VisibleComponent()
            {
                Color = Color.White,
                Source = animationFrames[0],
                Destination = animationFrames[0],
                Texture = spriteSheet
            };
            MoveableComponent moveable = new MoveableComponent();
            moveable.Mass = 15;
            ColliderComponent collider = new ColliderComponent()
            {
                Collider = new Rectangle(0, 0, 32, 32)
            };

            PlayerControllableComponent controllableComponent = new PlayerControllableComponent(new StaticKeyboardControls());
            GameObjectTag gameObjectTag = new GameObjectTag();

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
