using Microsoft.Xna.Framework.Graphics;
using TankEngine.Wrapper;

namespace Tank.GameStates.States
{
    /// <summary>
    /// Interface for the current game state
    /// </summary>
    public interface IState : IUpdateable, IDrawable, IRestoreable
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
        /// <param name="viewportAdapter">The viewport adapter to use</param>
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
        /// State is getting deleted
        /// </summary>
        void Destruct();
    }
}
