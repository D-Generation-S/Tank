using Microsoft.Xna.Framework;
using Tank.Components.Rendering;
using Tank.Events.EntityBased;
using Tank.Validator;

namespace Tank.Systems
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

            float currentTime = gameTime.ElapsedGameTime.Milliseconds;
            updateLocked = true;
            foreach (uint entityId in watchedEntities)
            {
                AnimationComponent animation = entityManager.GetComponent<AnimationComponent>(entityId);
                if (animation == null)
                {
                    return;
                }
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
                            RemoveEntityEvent removeEntityEvent = eventManager.CreateEvent<RemoveEntityEvent>();
                            removeEntityEvent.EntityId = entityId;
                            FireEvent(removeEntityEvent);
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
            updateLocked = false;
        }
    }
}
