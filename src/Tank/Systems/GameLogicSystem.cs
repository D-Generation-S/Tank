using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Tank.Builders;
using Tank.Components;
using Tank.Components.Tags;
using Tank.DataStructure;
using Tank.Events.EntityBased;
using Tank.Interfaces.EntityComponentSystem;
using Tank.Interfaces.EntityComponentSystem.Manager;
using Tank.Interfaces.MapGenerators;
using Tank.Utils;
using Tank.Validator;

namespace Tank.Systems
{
    /// <summary>
    /// The basic game logic system
    /// </summary>
    class GameLogicSystem : AbstractSystem
    {
        /// <summary>
        /// The players to spawn
        /// </summary>
        private readonly uint playerCount;

        /// <summary>
        /// The map width
        /// </summary>
        private readonly IMap map;

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
        /// The arrow entity to bind
        /// </summary>
        private uint arrowEntity;

        /// <summary>
        /// The order of the players
        /// </summary>
        private readonly int[] playerOrder;

        /// <summary>
        /// Create a new instance of this system
        /// </summary>
        public GameLogicSystem(uint playerCount, IMap map) : base()
        {
            validators.Add(new PlayerObjectValidator());
            playerOrder = new int[playerCount];
            this.playerCount = playerCount;
            setup = true;
            currentPlayerIndex = 0;
            this.map = map;
        }

        /// <<inheritdoc/>
        public override void Initialize(IGameEngine gameEngine)
        {
            base.Initialize(gameEngine);

            int playerSpace = map.Width / (int)(playerCount + 1);
            for (int i = 0; i < playerCount; i++)
            {
                int offset = i + 1;
                List<Rectangle> animationFrames = new List<Rectangle>();
                animationFrames.Add(new Rectangle(0, 0, 32, 32));
                Vector2 playerStartPosition = new Vector2(playerSpace * offset, 0);
                Raycast raycast = new Raycast(playerStartPosition, Vector2.UnitY, map.Height - 1);
                Position[] positions = raycast.GetPositions();
                for (int pIndex = positions.Length - 1; pIndex > 0; pIndex--)
                {
                    Position position = positions[pIndex];
                    if (!map.CollissionMap.GetValue(position))
                    {
                        playerStartPosition += Vector2.UnitY * position.Y;
                        break;
                    }
                }

                TankObjectBuilder tankObjectBuilder = new TankObjectBuilder(
                    new Position(playerStartPosition),
                    contentManager.Content.Load<Texture2D>("Images/Entities/BasicTank"),
                    animationFrames
                 );
                AddEntityEvent tankEntity = new AddEntityEvent(tankObjectBuilder.BuildGameComponents());
                eventManager.FireEvent(this, tankEntity);
            }
            List<IComponent> activePlayerArrow = new List<IComponent>()
            {
                new PlaceableComponent(50f, 50f),
                new VisibleComponent(Color.White, contentManager.Content.Load<Texture2D>("Images/Entities/BasicTank"))
            };
            arrowEntity = entityManager.CreateEntity(false);
            foreach (IComponent component in activePlayerArrow)
            {
                entityManager.AddComponent(arrowEntity, component);
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
                entityManager.AddComponent(watchedEntities[currentPlayerIndex], new BindComponent(arrowEntity, new Vector2(0f, -35f), false, true));
            }
        }

    }
}
