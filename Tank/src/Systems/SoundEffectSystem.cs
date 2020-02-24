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
        /// Create a new instance of this system
        /// </summary>
        public SoundEffectSystem() : base()
        {
            validators.Add(new SoundEffectValidator());
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
                soundEffect.SoundEffect.Play();
                entityManager.RemoveComponents(entityId, soundEffect);
            }
        }
    }
}
