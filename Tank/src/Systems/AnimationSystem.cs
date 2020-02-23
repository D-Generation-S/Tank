using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Tank.src.Components;
using Tank.src.Events.EntityBased;
using Tank.src.Validator;

namespace Tank.src.Systems
{
    /// <summary>
    /// This system will play amimations assigned to the entity
    /// </summary>
    class AnimationSystem : AbstractSystem
    {
        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        public AnimationSystem() : base()
        {
            validators.Add(new AnimationEntityValidator());
        }

        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            float currentTime = (float)gameTime.ElapsedGameTime.Milliseconds;
            List<uint> entitiesToRemove = new List<uint>();
            foreach (uint entityId in watchedEntities)
            {
                AnimationComponent animation = entityManager.GetComponent<AnimationComponent>(entityId);
                animation.TimeThreshold += currentTime / 1000;
                if (animation.TimeThreshold > animation.FrameSeconds)
                {
                    VisibleComponent visibleComponent = entityManager.GetComponent<VisibleComponent>(entityId);
                    animation.TimeThreshold = animation.TimeThreshold - animation.FrameSeconds;
                    animation.CurrentIndex += animation.ForwardDirection ? 1 : -1;
                    if (animation.CurrentIndex > animation.SpriteSources.Count - 1)
                    {
                        if (!animation.Loop)
                        {
                            entitiesToRemove.Add(entityId);

                            continue;
                        }
                        animation.CurrentIndex = 0;
                        if (animation.PingPong)
                        {
                            if (animation.ForwardDirection)
                            {
                                animation.CurrentIndex = animation.SpriteSources.Count - 2;
                            }
                            animation.ForwardDirection = false;
                        }
                    }
                    if (!animation.ForwardDirection && animation.CurrentIndex < 0)
                    {
                        animation.CurrentIndex = 1;
                        animation.ForwardDirection = true;
                    }
                    visibleComponent.Source = animation.SpriteSources[animation.CurrentIndex];

                }
            }

            foreach (uint entityId in entitiesToRemove)
            {
                FireEvent<RemoveEntityEvent>(new RemoveEntityEvent(entityId));
            }
        }
    }
}
