using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tank.Wrapper;

namespace Tank.GameStates.States
{
    /// <summary>
    /// Interface for the current game state
    /// </summary>
    interface IState : IUpdateable, IDrawable
    {
        /// <summary>
        /// Is this game state already initialize
        /// </summary>
        bool Initialized { get; }

        /// <summary>
        /// Set the game state manager
        /// </summary>
        /// <param name="gameStateManager">The game state manager to use</param>
        void SetGameStateManager(GameStateManager gameStateManager);

        /// <summary>
        /// Initialize the game state this is called first
        /// </summary>
        /// <param name="contentWrapper">The content wrapper to use</param>
        /// <param name="spriteBatch">Sprite batch to use for drawing</param>
        void Initialize(ContentWrapper contentWrapper, SpriteBatch spriteBatch);

        /// <summary>
        /// Load the content for the state this is called second
        /// </summary>
        void LoadContent();

        /// <summary>
        /// Set this state to active, this is called right before the state goes live
        /// </summary>
        void SetActive();

        /// <summary>
        /// Restore this state
        /// </summary>
        void Restore();

        /// <summary>
        /// Suspend this state
        /// </summary>
        void Suspend();

        /// <summary>
        /// State is getting deleted
        /// </summary>
        void Destruct();
    }
}
