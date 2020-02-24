using Microsoft.Xna.Framework.Audio;

namespace Tank.src.Interfaces.Factories
{
    /// <summary>
    /// This interface defines a factory to return random sound effects
    /// </summary>
    interface ISoundFactory
    {
        /// <summary>
        /// This method will return you an random sound effect
        /// </summary>
        /// <returns></returns>
        SoundEffect GetRandomSoundEffect();
    }
}
