using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Tank.Wrapper;

namespace Tank.GameStates
{
    /// <summary>
    /// Manager to manage the game state
    /// </summary>
    class GameStateManager : IDrawable, IUpdateable
    {
        /// <summary>
        /// Stack to use for all the game states
        /// </summary>
        Stack<IState> stateStack;

        /// <summary>
        /// The content wrapper to use for state init
        /// </summary>
        private readonly ContentWrapper contentWrapper;

        /// <summary>
        /// The spritebatch to use for state init
        /// </summary>
        private readonly SpriteBatch spriteBatch;

        /// <summary>
        /// Is there a game state available
        /// </summary>
        public bool StateAvailable { get; private set; }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="contentWrapper">The content wrapper to use</param>
        /// <param name="spriteBatch">The spritebatch to use</param>
        public GameStateManager(ContentWrapper contentWrapper, SpriteBatch spriteBatch)
        {
            stateStack = new Stack<IState>();
            this.contentWrapper = contentWrapper;
            this.spriteBatch = spriteBatch;
        }

        /// <summary>
        /// Clear all the states and add a new one
        /// </summary>
        /// <param name="state">The new only state to add</param>
        public void ResetState(IState state)
        {
            stateStack.Clear();
            Add(state);
        }

        /// <summary>
        /// Add a new state on top of the current one
        /// </summary>
        /// <param name="state">The state to add</param>
        /// <returns>True if adding was successful</returns>
        public bool Add(IState state)
        {
            if (stateStack.Contains(state))
            {
                return false;
            }
            if (!state.Initialized)
            {
                state.SetGameStateManager(this);
                state.Initialize(contentWrapper, spriteBatch);
                state.LoadContent();
                state.SetActive();
            }
            stateStack.Push(state);
            return true;
        }

        /// <summary>
        /// Replace the current state
        /// </summary>
        /// <param name="state">The new state to add</param>
        /// <returns>True if replacement was successful</returns>
        public bool Replace(IState state)
        {
            Pop();
            return Add(state);
        }

        /// <summary>
        /// Remove the last state
        /// </summary>
        /// <returns>True if pop was successful</returns>
        public void Pop()
        {
            if (stateStack.Count == 0)
            {
                return;
            }
            IState state = stateStack.Pop();
            state.Destruct();
            return;
        }

        /// <inheritdoc/>
        public void Draw(GameTime gameTime)
        {
            if (stateStack.Count == 0)
            {
                StateAvailable = false;
                return;
            }
            IState currentState = stateStack.Peek();
            currentState.Draw(gameTime);
            StateAvailable = true;
        }

        /// <inheritdoc/>
        public void Update(GameTime gameTime)
        {
            if (stateStack.Count == 0)
            {
                StateAvailable = false;
                return;
            }
            IState currentState = stateStack.Peek();
            currentState.Update(gameTime);
            StateAvailable = true;
        }
    }
}
