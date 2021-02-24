using Microsoft.Xna.Framework.Audio;

namespace Tank.Components
{
    class SoundEffectComponent : BaseComponent
    {
        /// <summary>
        /// Name of the sound effect
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Public access if there should be a random pitch
        /// </summary>
        public bool RandomPitch { get; set; }

        /// <summary>
        /// Public readonly access to the sound effect
        /// </summary>
        public SoundEffect SoundEffect { get; set; }

        /// <inheritdoc/>
        public override void Init()
        {
            Name = string.Empty;
            RandomPitch = false;
            SoundEffect = null;
        }
    }
}
