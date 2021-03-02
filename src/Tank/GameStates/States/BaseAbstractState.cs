using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        protected GameStateManager gameStateManager;
        protected ContentWrapper contentWrapper;
        protected SpriteBatch spriteBatch;

        public virtual void Initialize(ContentWrapper contentWrapper, SpriteBatch spriteBatch)
        {
            this.contentWrapper = contentWrapper;
            this.spriteBatch = spriteBatch;
            Initialized = true;
        }

        abstract public void LoadContent();

        public virtual void SetActive()
        {
        }

        public void SetGameStateManager(GameStateManager gameStateManager)
        {
            this.gameStateManager = gameStateManager;
        }

        public virtual void Draw(GameTime gameTime)
        {
        }

        public virtual void Update(GameTime gameTime)
        {
        }

        public virtual void Destruct()
        {
        }
    }
}
