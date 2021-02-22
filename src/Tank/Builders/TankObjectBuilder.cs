using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Tank.Components;
using Tank.src.Components.Tags;
using Tank.src.DataStructure;
using Tank.src.Interfaces.Builders;
using Tank.src.Interfaces.EntityComponentSystem;

namespace Tank.src.Builders
{
    class TankObjectBuilder : IGameObjectBuilder
    {
        private readonly Position startPosition;
        private readonly Texture2D spriteSheet;
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

        public List<IComponent> BuildGameComponents()
        {
            List<IComponent> returnComponents = new List<IComponent>();
            PlaceableComponent placeableComponent = new PlaceableComponent()
            {
                Position = startPosition.GetVector2()
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
