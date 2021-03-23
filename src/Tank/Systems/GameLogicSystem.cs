using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Tank.Components;
using Tank.Components.Rendering;
using Tank.Components.Tags;
using Tank.Enums;
using Tank.Events;
using Tank.Events.ComponentBased;
using Tank.Events.EntityBased;
using Tank.Events.StateEvents;
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
        /// The current player id
        /// </summary>
        private uint currentPlayerId;

        /// <summary>
        /// The arrow entity to bind
        /// </summary>
        private uint arrowEntity;

        /// <summary>
        /// The order of the players
        /// </summary>
        private readonly int[] playerOrder;

        /// <summary>
        /// The current game settings
        /// </summary>
        private readonly GameSettings settings;

        /// <summary>
        /// The current game state
        /// </summary>
        private GameStatesEnum currentGameStateEnum;

        /// <summary>
        /// Create a new instance of this system
        /// </summary>
        public GameLogicSystem(GameSettings settings) : base()
        {
            validators.Add(new GameObjectValidator());
            playerOrder = new int[settings.PlayerCount];
            setup = true;
            currentPlayerIndex = 0;
            this.settings = settings;
            currentGameStateEnum = GameStatesEnum.Unknown;
        }

        /// <<inheritdoc/>
        public override void Initialize(IGameEngine gameEngine)
        {
            base.Initialize(gameEngine);
            eventManager.SubscribeEvent(this, typeof(RemoveEntityEvent));

            arrowEntity = entityManager.CreateEntity(false);
            entityManager.CreateComponent<PlaceableComponent>(arrowEntity);

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


            VisibleComponent arrowVisible = entityManager.CreateComponent<VisibleComponent>(arrowEntity);
            arrowVisible.Texture = contentManager.Load<Texture2D>("Images/Entities/SelectionArrow");
            arrowVisible.DrawMiddle = true;
            arrowVisible.Destination = new Rectangle(0, 0, arrowVisible.Texture.Width, arrowVisible.Texture.Height);
            arrowVisible.Source = animationComponent.SpriteSources[0];

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

        public override void EventNotification(object sender, IGameEvent eventArgs)
        {
            base.EventNotification(sender, eventArgs);
            if (eventArgs is RemoveEntityEvent entityToRemove)
            {
                if (watchedEntities.Contains(entityToRemove.EntityId))
                {
                    SetNextPlayer();
                }
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

            if (watchedEntities.Count <= 1)
            {
                // Points who did won and other important stuff should be added to the event
                GameOverEvent gameOverEvent = eventManager.CreateEvent<GameOverEvent>();
                FireEvent(gameOverEvent);
                return;
            }

            if (!activePlayer)
            {
                activePlayer = true;
                currentPlayerId = watchedEntities[currentPlayerIndex];
                entityManager.CreateComponent<ActiveGameObjectTag>(currentPlayerId);
                entityManager.CreateComponent<CanPerformActionTag>(currentPlayerId);
                BindComponent bindComponent = entityManager.CreateComponent<BindComponent>();
                bindComponent.DeleteIfParentGone = false;
                bindComponent.BoundEntityId = currentPlayerId;
                bindComponent.Offset = Vector2.UnitY * -60;
                bindComponent.Source = true;
                bindComponent.PositionBound = true;

                entityManager.AddComponent(arrowEntity, bindComponent, true);
                FireGameStateChange(GameStatesEnum.RoundStart);
                return;
            }

            if (currentGameStateEnum == GameStatesEnum.RoundStart)
            {
                FireGameStateChange(GameStatesEnum.RoundRunning);
            }

            if ((activePlayer && !watchedEntities.Contains(currentPlayerId))
                || (entityManager.GetEntitiesWithComponent<CanPerformActionTag>().Count == 0
                && entityManager.GetEntitiesWithComponent<RoundBlockingTag>().Count == 0))
            {
                SetNextPlayer();
            }
        }

        private void SetNextPlayer()
        {
            currentPlayerIndex++;
            if (currentPlayerIndex > watchedEntities.Count - 1)
            {
                currentPlayerIndex = 0;
            }

            FireGameStateChange(GameStatesEnum.RoundEnd);
            uint oldPlayerId = currentPlayerId;
            currentPlayerId = watchedEntities[currentPlayerIndex];

            BindComponent bindComponent = entityManager.GetComponent<BindComponent>(arrowEntity);
            entityManager.RemoveComponents(oldPlayerId, typeof(ActiveGameObjectTag));
            entityManager.CreateComponent<ActiveGameObjectTag>(currentPlayerId);
            entityManager.CreateComponent<CanPerformActionTag>(currentPlayerId);

            bindComponent.BoundEntityId = currentPlayerId;
            FireGameStateChange(GameStatesEnum.RoundStart);
        }

        private void FireGameStateChange(GameStatesEnum gameStateEnum)
        {
            currentGameStateEnum = gameStateEnum;
            GameStateChangedEvent stateChanged = eventManager.CreateEvent<GameStateChangedEvent>();
            stateChanged.GameStateEnum = gameStateEnum;
            stateChanged.EntityId = currentPlayerId;
            FireEvent(stateChanged);
        }

    }
}
