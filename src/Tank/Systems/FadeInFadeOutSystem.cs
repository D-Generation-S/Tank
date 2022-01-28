﻿using Microsoft.Xna.Framework;
using Tank.Components;
using Tank.Components.Rendering;
using Tank.Validator;
using TankEngine.EntityComponentSystem.Events;
using TankEngine.EntityComponentSystem.Manager;
using TankEngine.EntityComponentSystem.Systems;

namespace Tank.Systems
{
    /// <summary>
    /// This system will handle the fade in and fade out of objects
    /// </summary>
    class FadeInFadeOutSystem : AbstractSystem
    {
        /// <inheritdoc/>
        public override void Initialize(IGameEngine gameEngine)
        {
            base.Initialize(gameEngine);
            validators.Add(new FadeableValidator());
        }

        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {
            foreach (uint entityId in watchedEntities)
            {

                BaseVisibleComponent visibleComponent = entityManager.GetComponent<VisibleComponent>(entityId);
                visibleComponent = visibleComponent ?? entityManager.GetComponent<VisibleTextComponent>(entityId);
                FadeComponent fadeComponent = entityManager.GetComponent<FadeComponent>(entityId);
                if (visibleComponent == null || fadeComponent == null || entitiesToRemove.Contains(entityId))
                {
                    break;
                }

                if (fadeComponent.TicksToLive <= 0 && visibleComponent.Color.A <= fadeComponent.StartOpacity)
                {

                    RemoveEntityEvent removeEntityEvent = CreateEvent<RemoveEntityEvent>();
                    removeEntityEvent.EntityId = entityId;
                    FireEvent(removeEntityEvent);
                    break;
                }

                if (fadeComponent.TicksToLive > 0 && visibleComponent.Color.A < fadeComponent.TargetOpacity)
                {
                    FadeIn(visibleComponent, fadeComponent);
                    continue;
                }

                if (fadeComponent.TicksToLive <= 0)
                {
                    FadeOut(visibleComponent, fadeComponent);
                    continue;
                }

                fadeComponent.TicksToLive--;

            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Fade in the object
        /// </summary>
        /// <param name="visibleComponent">The visible component to use</param>
        /// <param name="fadeComponent">The face component to use</param>
        private void FadeIn(BaseVisibleComponent visibleComponent, FadeComponent fadeComponent)
        {
            fadeComponent.RealOpacityChange += fadeComponent.OpacityChange; if (fadeComponent.RealOpacityChange > 1)
            {
                fadeComponent.RealOpacityChange -= 1;
                if (visibleComponent.Color.A < fadeComponent.TargetOpacity)
                {
                    visibleComponent.Color.A += 1;
                }
            }
        }

        /// <summary>
        /// Fade out the object
        /// </summary>
        /// <param name="visibleComponent">The visible component to use</param>
        /// <param name="fadeComponent">The fade component to use</param>
        private void FadeOut(BaseVisibleComponent visibleComponent, FadeComponent fadeComponent)
        {
            fadeComponent.RealOpacityChange += fadeComponent.OpacityChange;
            if (fadeComponent.RealOpacityChange > 1)
            {
                fadeComponent.RealOpacityChange -= 1;
                if (visibleComponent.Color.A > 0)
                {
                    visibleComponent.Color.A -= 1;
                }
            }
        }
    }
}