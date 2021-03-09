using Microsoft.Xna.Framework;
using System;
using Tank.Components;
using Tank.DataStructure.Settings;
using Tank.Validator;

namespace Tank.Systems
{
    /// <summary>
    /// This system will play sound effects 
    /// </summary>
    class SoundEffectSystem : AbstractSystem
    {
        /// <summary>
        /// Random class to use to change the pitch if needed
        /// </summary>
        private readonly Random random;

        /// <summary>
        /// The application settings to use
        /// </summary>
        private readonly ApplicationSettings applicationSettings;

        /// <summary>
        /// Create a new instance of this system
        /// </summary>
        public SoundEffectSystem(ApplicationSettings applicationSettings) : base()
        {
            validators.Add(new SoundEffectValidator());
            random = new Random();
            this.applicationSettings = applicationSettings;
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
                soundEffect.SoundEffect.Play(applicationSettings.EffectVolume, pitch, 0f);
                entityManager.RemoveComponents(entityId, soundEffect);
            }
        }
    }
}
