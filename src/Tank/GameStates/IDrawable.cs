using Microsoft.Xna.Framework;

namespace Tank.GameStates
{
    /// <summary>
    /// If a thing is drawable
    /// </summary>
    interface IDrawable
    {
        /// <summary>
        /// Draw everything if needed
        /// </summary>
        /// <param name="gameTime">The current game time</param>
        void Draw(GameTime gameTime);
    }
}
