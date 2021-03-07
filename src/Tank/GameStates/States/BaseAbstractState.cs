﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tank.Adapter;
using Tank.DataStructure.Settings;
using Tank.Wrapper;

namespace Tank.GameStates.States
{
    /// <summary>
    /// Abstract game state
    /// </summary>
    abstract class BaseAbstractState : IState
    {
        /// <inheritdoc/>
        public bool Initialized { get; protected set; }

        /// <summary>
        /// The game state manager to use
        /// </summary>
        protected GameStateManager gameStateManager;

        /// <summary>
        /// The content wrapper to use
        /// </summary>
        protected ContentWrapper contentWrapper;

        /// <summary>
        /// The viewport adapter
        /// </summary>
        protected IViewportAdapter viewportAdapter => TankGame.PublicViewportAdapter;

        /// <summary>
        /// Mouse wrapper to use
        /// </summary>
        protected MouseWrapper mouseWrapper;

        /// <summary>
        /// The spritebatch to use
        /// </summary>
        protected SpriteBatch spriteBatch;

        /// <summary>
        /// The settings for the application
        /// </summary>
        protected ApplicationSettings settings;

        /// <inheritdoc/>
        public virtual void Initialize(ContentWrapper contentWrapper, SpriteBatch spriteBatch, ApplicationSettings applicationSettings)
        {
            this.contentWrapper = contentWrapper;
            this.spriteBatch = spriteBatch;
            mouseWrapper = new MouseWrapper(viewportAdapter);
            settings = applicationSettings;
            Initialized = true;
        }

        /// <summary>
        /// Get the current scale matrix
        /// </summary>
        /// <returns></returns>
        protected Matrix GetScaleMatrix()
        {
            return viewportAdapter.GetScaleMatrix();
        }

        /// <inheritdoc/>
        abstract public void LoadContent();

        /// <inheritdoc/>
        public virtual void SetActive()
        {
            viewportAdapter.Reset();
        }

        /// <inheritdoc/>
        public void SetGameStateManager(GameStateManager gameStateManager)
        {
            this.gameStateManager = gameStateManager;
        }

        /// <inheritdoc/>
        public virtual void Restore()
        {
        }

        /// <inheritdoc/>
        public virtual void Suspend()
        {
        }

        /// <inheritdoc/>
        public virtual void Draw(GameTime gameTime)
        {
        }

        /// <inheritdoc/>
        public virtual void Update(GameTime gameTime)
        {
        }

        /// <inheritdoc/>
        public virtual void Destruct()
        {
        }
    }
}
