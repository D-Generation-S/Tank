using Microsoft.Xna.Framework;

namespace Tank.GameStates
{
    /// <summary>
    /// Something is updateable
    /// </summary>
    interface IUpdateable
    {
        /// <summary>
        /// Run a game update
        /// </summary>
        /// <param name="gameTime">Current game time</param>
        void Update(GameTime gameTime);
    }
}
