using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tank.src.Components;
using Tank.src.Validator;

namespace Tank.src.Systems
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
        /// Create a new instance of this system
        /// </summary>
        public SoundEffectSystem() : base()
        {
            validators.Add(new SoundEffectValidator());
            random = new Random();
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
                float pitch = 0f;
                if (soundEffect.RandomPitch)
                {
                    pitch = random.Next(-15, 15);
                    pitch /= 100;
                }
                soundEffect.SoundEffect.Play(1f, pitch, 0f);
                entityManager.RemoveComponents(entityId, soundEffect);
            }
        }
    }
}
