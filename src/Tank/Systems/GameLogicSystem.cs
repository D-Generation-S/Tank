using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Tank.Components;
using Tank.Components.Rendering;
using Tank.Components.Tags;
using Tank.Events.ComponentBased;
using Tank.GameStates.Data;
using Tank.Interfaces.EntityComponentSystem.Manager;
using Tank.Validator;

namespace Tank.Systems
{
    /// <summary>
    /// The basic game logic system
    /// </summary>
    class GameLogicSystem : AbstractSystem
    {
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
        private readonly GameSettings settings;

        /// <summary>
        /// Create a new instance of this system
        /// </summary>
        public GameLogicSystem(GameSettings settings) : base()
        {
            validators.Add(new PlayerObjectValidator());
            playerOrder = new int[settings.PlayerCount];
            setup = true;
            currentPlayerIndex = 0;
            this.settings = settings;
        }

        /// <<inheritdoc/>
        public override void Initialize(IGameEngine gameEngine)
        {
            base.Initialize(gameEngine);

            arrowEntity = entityManager.CreateEntity(false);
            entityManager.CreateComponent<PlaceableComponent>(arrowEntity);
            VisibleComponent arrowVisible = entityManager.CreateComponent<VisibleComponent>(arrowEntity);
            arrowVisible.Texture = contentManager.Load<Texture2D>("Images/Entities/SelectionArrow");
            arrowVisible.Destination = new Rectangle(0, 0, arrowVisible.Texture.Width, arrowVisible.Texture.Height);
            arrowVisible.Source = new Rectangle(0, 0, arrowVisible.Texture.Width, arrowVisible.Texture.Height);
            arrowVisible.DrawMiddle = true;

            AnimationComponent animationComponent = entityManager.CreateComponent<AnimationComponent>(arrowEntity);
            animationComponent.FrameSeconds = 0.25f;
            animationComponent.Active = true;
            animationComponent.Name = "Idle";
            animationComponent.Loop = true;
            animationComponent.PingPong = true;
            animationComponent.SpriteSources = new List<Rectangle>()
            {
                new Rectangle(0, 0, 32, 32),
                new Rectangle(32, 0, 32, 32),
                new Rectangle(0, 32, 32, 32),
                new Rectangle(32, 32, 32, 32)
            };

            ComponentChangedEvent componentChangedEvent = eventManager.CreateEvent<ComponentChangedEvent>();
            componentChangedEvent.EntityId = arrowEntity;
            FireEvent(componentChangedEvent);
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
                if (watchedEntities.Count != this.settings.PlayerCount)
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
                entityManager.AddComponent(watchedEntities[currentPlayerIndex], new BindComponent()
                {
                    BoundEntityId = arrowEntity,
                    Offset = new Vector2(0f, -60f),
                    PositionBound = true
                });
            }
        }

    }
}
