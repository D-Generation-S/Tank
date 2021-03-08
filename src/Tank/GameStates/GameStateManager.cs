using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Tank.DataStructure.Settings;
using Tank.GameStates.States;
using Tank.Wrapper;

namespace Tank.GameStates
{
    /// <summary>
    /// Manager to manage the game state
    /// </summary>
    class GameStateManager : IDrawable, IUpdateable, IRestoreable
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
        /// The application settings to use
        /// </summary>
        private readonly ApplicationSettings applicationSettings;

        /// <summary>
        /// Is the state manager suspended
        /// </summary>
        protected bool isSuspended;

        /// <summary>
        /// Is there a game state available
        /// </summary>
        public bool StateAvailable => stateStack.Count > 0;

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="contentWrapper">The content wrapper to use</param>
        /// <param name="spriteBatch">The spritebatch to use</param>
        /// <param name="viewportAdapter">The viewport adapter to use</param>
        public GameStateManager(ContentWrapper contentWrapper, SpriteBatch spriteBatch, ApplicationSettings applicationSettings)
        {
            stateStack = new Stack<IState>();
            this.contentWrapper = contentWrapper;
            this.spriteBatch = spriteBatch;
            this.applicationSettings = applicationSettings;
            isSuspended = false;
        }

        /// <summary>
        /// Clear all the states and add a new one
        /// </summary>
        /// <param name="state">The new only state to add</param>
        public void ResetState(IState state)
        {
            Clear();
            Add(state);
        }

        /// <summary>
        /// Add a new state on top of the current one
        /// </summary>
        /// <param name="state">The state to add</param>
        /// <returns>True if adding was successful</returns>
        public bool Add(IState state)
        {
            return Add(state, true);
        }

        /// <summary>
        /// Add a new state on top of the current one
        /// </summary>
        /// <param name="state">The state to add</param>
        /// <param name="shouldSuspend">Should the state be suspended</param>
        /// <returns>True if adding was successful</returns>
        public bool Add(IState state, bool shouldSuspend)
        {
            if (stateStack.Contains(state))
            {
                return false;
            }
            if (state.Initialized)
            {
                state.Restore();
            }
            if (!state.Initialized)
            {
                state.SetGameStateManager(this);
                state.Initialize(contentWrapper, spriteBatch, applicationSettings);
                state.LoadContent();
                state.SetActive();
            }
            if (shouldSuspend && StateAvailable)
            {
                stateStack.Peek().Suspend();
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
            if (stateStack.Count > 0)
            {
                stateStack.Peek().Restore();
            }
            return;
        }

        /// <summary>
        /// Clear the whole game state manager
        /// </summary>
        public void Clear()
        {
            stateStack.Clear();
        }

        /// <inheritdoc/>
        public void Draw(GameTime gameTime)
        {
            if (!StateAvailable)
            {
                //StateAvailable = false;
                return;
            }
            IState currentState = stateStack.Peek();
            currentState.Draw(gameTime);
            //StateAvailable = true;
        }

        /// <inheritdoc/>
        public void Update(GameTime gameTime)
        {
            if (!StateAvailable)
            {
                //StateAvailable = false;
                return;
            }
            IState currentState = stateStack.Peek();
            currentState.Update(gameTime);
            //StateAvailable = true;
        }

        public void Restore()
        {
            if (!isSuspended || !StateAvailable)
            {
                return;
            }
            isSuspended = false;
            stateStack.Peek().Restore();
        }

        public void Suspend()
        {
            if (isSuspended || !StateAvailable)
            {
                return;
            }

            isSuspended = true;
            stateStack.Peek().Suspend();
        }
    }
}
