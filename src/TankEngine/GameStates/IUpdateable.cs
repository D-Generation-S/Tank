using Microsoft.Xna.Framework;

namespace TankEngine.GameStates
{
    /// <summary>
    /// Something is updateable
    /// </summary>
    public interface IUpdateable
    {
        /// <summary>
        /// Run a game update
        /// </summary>
        /// <param name="gameTime">Current game time</param>
        void Update(GameTime gameTime);
    }
}
