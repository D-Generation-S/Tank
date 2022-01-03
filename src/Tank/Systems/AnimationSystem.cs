using Microsoft.Xna.Framework;
using Tank.Components.Rendering;
using Tank.Validator;
using TankEngine.EntityComponentSystem.Events;
using TankEngine.EntityComponentSystem.Systems;

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
                    Rectangle spriteSource = animation.SpriteSources[animation.CurrentIndex];
                    spriteSource.Width -= visibleComponent.CutoffRight;
                    Rectangle destination = visibleComponent.Destination;
                    destination.Width = visibleComponent.SingleTextureSize.Width - visibleComponent.CutoffRight;

                    visibleComponent.Destination = destination;
                    visibleComponent.Source = spriteSource;
                }
            }
            updateLocked = false;
        }
    }
}
