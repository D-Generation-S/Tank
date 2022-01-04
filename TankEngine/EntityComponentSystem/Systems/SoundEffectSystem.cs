using Microsoft.Xna.Framework;
using System;
using TankEngine.EntityComponentSystem.Components.Sound;
using TankEngine.EntityComponentSystem.Events;
using TankEngine.EntityComponentSystem.Manager;
using TankEngine.EntityComponentSystem.Validator;

namespace TankEngine.EntityComponentSystem.Systems
{
    /// <summary>
    /// This system will play sound effects 
    /// </summary>
    public class SoundEffectSystem : AbstractSystem
    {
        /// <summary>
        /// Random class to use to change the pitch if needed
        /// </summary>
        private readonly Random random;

        private float effectVolume;

        /// <summary>
        /// Create a new instance of this system
        /// </summary>
        public SoundEffectSystem(float effectVolume) : base()
        {
            validators.Add(new SoundEffectValidator());
            random = new Random();
            this.effectVolume = MathHelper.Clamp(effectVolume, 0, 1);
        }

        /// <inheritdoc/>
        public override void Initialize(IGameEngine gameEngine)
        {
            base.Initialize(gameEngine);
            gameEngine.EventManager.SubscribeEvent<VolumeChangedEvent>(this);
        }

        /// <inheritdoc/>
        public override void EventNotification(object sender, IGameEvent eventArgs)
        {
            if (eventArgs is VolumeChangedEvent volumeChanged)
            {
                switch (volumeChanged.VolumeType)
                {
                    case Enums.VolumeTypeEnum.Effect:
                        effectVolume = volumeChanged.NewVolume;
                        break;
                }
            }
        }

        /// <summary>
        /// Update this system
        /// </summary>
        /// <param name="gameTime">The current game time</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            foreach (uint entityId in watchedEntities)
            {
                SoundEffectComponent soundEffect = entityManager.GetComponent<SoundEffectComponent>(entityId);
                if (soundEffect == null)
                {
                    continue;
                }
                float pitch = 0f;
                if (soundEffect.RandomPitch)
                {
                    pitch = random.Next(-15, 15);
                    pitch /= 100;
                }
                if (soundEffect.SoundEffect == null)
                {
                    return;
                }
                soundEffect.SoundEffect.Play(effectVolume, pitch, 0f);
                entityManager.RemoveComponents(entityId, soundEffect);
            }
        }
    }
}
