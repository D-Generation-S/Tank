using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tank.src.Builders;
using Tank.src.Components.Tags;
using Tank.src.DataStructure;
using Tank.src.Events.ComponentBased;
using Tank.src.Events.EntityBased;
using Tank.src.Interfaces.EntityComponentSystem.Manager;
using Tank.src.Validator;

namespace Tank.src.Systems
{
    class GameLogicSystem : AbstractSystem
    {
        /// <summary>
        /// The players to spawn
        /// </summary>
        private readonly uint playerCount;

        /// <summary>
        /// Is the system in setup mode
        /// </summary>
        private bool setup;

        /// <summary>
        /// Is there an acitve player set
        /// </summary>
        private bool activePlayer;

        /// <summary>
        /// The current player
        /// </summary>
        private int currentPlayerIndex;

        /// <summary>
        /// The order of the players
        /// </summary>
        private readonly int[] playerOrder;

        /// <summary>
        /// Create a new instance of this system
        /// </summary>
        public GameLogicSystem(uint playerCount) : base()
        {
            validators.Add(new PlayerObjectValidator());
            playerOrder = new int[playerCount];
            this.playerCount = playerCount;
            setup = true;
            currentPlayerIndex = 0;
        }

        /// <<inheritdoc/>
        public override void Initialize(IGameEngine gameEngine)
        {
            base.Initialize(gameEngine);

            for (int i = 0; i < playerCount; i++)
            {
                int offset = i + 1;
                List<Rectangle> animationFrames = new List<Rectangle>();
                animationFrames.Add(new Rectangle(0, 0, 32, 32));
                TankObjectBuilder tankObjectBuilder = new TankObjectBuilder(
                    new Position(100 * offset, 0),
                    contentManager.Content.Load<Texture2D>("Images/Entities/BasicTank"),
                    animationFrames
                 );
                AddEntityEvent tankEntity = new AddEntityEvent(tankObjectBuilder.BuildGameComponents());
                eventManager.FireEvent<AddEntityEvent>(this, tankEntity);
            }

        }

        /// <summary>
        /// Shuffle the player list so that the player order is mixed
        /// </summary>
        private void ShuffleList()
        {
            Random random = new Random();

            for (int i = playerOrder.Length - 1; i >= 0; i--)
            {
                int k = random.Next(i + 1);
                uint tempValue = watchedEntities[k];
                watchedEntities[k] = watchedEntities[i];
                watchedEntities[i] = tempValue;
            }
        }

        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (setup)
            {
                if (watchedEntities.Count != playerCount)
                {
                    return;
                }
                ShuffleList();
            }

            setup = false;

            if (watchedEntities.Count == 0)
            {
                //Game is over!
                return;
            }

            if (!activePlayer)
            {
                activePlayer = true;
                entityManager.AddComponent(watchedEntities[currentPlayerIndex], new ActiveGameObjectTag());
            }
        }

    }
}
