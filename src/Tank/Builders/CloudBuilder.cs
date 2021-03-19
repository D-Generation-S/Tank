using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Tank.Components;
using Tank.Components.Rendering;
using Tank.Components.Tags;
using Tank.Interfaces.EntityComponentSystem;
using Tank.Interfaces.Randomizer;

namespace Tank.Builders
{
    /// <summary>
    /// A class to build the clouds for the game
    /// </summary>
    class CloudBuilder : BaseBuilder
    {
        private readonly Texture2D texture;
        private readonly List<Rectangle> animationFrames;
        private readonly IRandomizer randomizer;
        private readonly Rectangle spawnArea;
        private Rectangle destination;

        public CloudBuilder(Texture2D texture, List<Rectangle> animationFrames, IRandomizer randomizer, Rectangle spawnArea)
        {
            this.texture = texture;
            this.animationFrames = animationFrames;
            this.randomizer = randomizer;
            this.spawnArea = spawnArea;
            destination = new Rectangle(0, 0, animationFrames[0].Width, animationFrames[0].Height);
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
            placeableComponent.Position = Vector2.UnitY * randomizer.GetNewNumber(spawnArea.Y, spawnArea.Height);
            placeableComponent.Position += Vector2.UnitX * randomizer.GetNewNumber(spawnArea.X, spawnArea.Width);
            MoveableComponent moveableComponent = entityManager.CreateComponent<MoveableComponent>();
            moveableComponent.Velocity = Vector2.UnitX * randomizer.GetNewNumber(0.5f, 2);
            VisibleComponent visibleComponent = entityManager.CreateComponent<VisibleComponent>();
            visibleComponent.Texture = texture;
            visibleComponent.Destination = destination;
            visibleComponent.Source = animationFrames[0];
            visibleComponent.SingleTextureSize = destination;
            visibleComponent.DrawMiddle = true;
            visibleComponent.Color.A = 0;
            AnimationComponent animationComponent = entityManager.CreateComponent<AnimationComponent>();
            animationComponent.Active = true;
            animationComponent.Name = "Idle";
            animationComponent.Loop = true;
            animationComponent.SpriteSources = animationFrames;
            FadeComponent fadeComponent = entityManager.CreateComponent<FadeComponent>();
            fadeComponent.TicksToLive = randomizer.GetNewIntNumber(120, 300);
            fadeComponent.TargetOpacity = randomizer.GetNewIntNumber(120, 170);

            MoveableOnlyTag moveableOnlyTag = entityManager.CreateComponent<MoveableOnlyTag>();
            CloudTag cloudTag = entityManager.CreateComponent<CloudTag>();

            returnList.Add(visibleComponent);
            returnList.Add(moveableOnlyTag);
            returnList.Add(cloudTag);
            returnList.Add(animationComponent);
            returnList.Add(placeableComponent);
            returnList.Add(moveableComponent);
            returnList.Add(fadeComponent);
            return returnList;
        }
    }
}
