using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using Tank.Builders;
using Tank.Components;
using Tank.Components.Rendering;
using Tank.Components.Tags;
using Tank.Events.PhysicBased;
using Tank.Events.StateEvents;
using Tank.Events.TerrainEvents;
using Tank.Factories;
using Tank.GameStates.Data;
using Tank.Interfaces.Builders;
using Tank.Map.Generators;
using TankEngine.DataStructures.Geometrics;
using TankEngine.EntityComponentSystem;
using TankEngine.EntityComponentSystem.Events;
using TankEngine.EntityComponentSystem.Manager;
using TankEngine.GameStates.States;
using TankEngine.Randomizer;
using TankEngine.Wrapper;

namespace Tank.GameStates.States
{
    /// <summary>
    /// The game state class
    /// Since this is not done just yet the summary blocks are missing right now
    /// </summary>
    class GameState : BaseAbstractState, IEventReceiver
    {
        private IState gameOverState;

        private int ticksToFire;

        private KeyboardState previousState;
        private MouseState previousMouseState;

        private RandomEntityBuilderFactory randomExplosionFactory;
        private RandomEntityBuilderFactory randomCloudFactory;

        private BaseBulletBuilder bulletBuilder;
        private MapDebriBuilder debriBuilder;

        private List<Rectangle> projectiveAnimationFrames;
        private List<Rectangle> explosionAnimationFrames;

        private List<SoundEffect> explosionSounds;

        Vector2 bulletSpawnLocation;

        private Texture2D bulletTestExplosion;
        private Texture2D bulletTest;
        private Texture2D clouds;
        private Texture2D pixelTexture;
        private SpriteFont gameFont;

        IGameEngine engine;
        //private readonly IMap mapToUse;
        private readonly GameSettings gameSettings;
        private readonly IRandomizer randomizer;

        private uint mapId;
        private uint entityCounter;
        private bool debugOn;
        private bool debugIdGenerated;

        private bool openMenu;

        private readonly int cloudsToSpawn;

        private float fps;

        public GameState(IGameEngine gameEngine, GameSettings gameSettings)
        {
            //this.mapToUse = mapToUse;
            this.gameSettings = gameSettings;
            this.engine = gameEngine;
            randomizer = new SystemRandomizer();
            randomizer.Initzialize(100);
            debugOn = gameSettings.IsDebug;
            debugIdGenerated = false;
            cloudsToSpawn = 250;
            openMenu = false;
        }

        /// <inheritdoc/>
        public override void Initialize(ContentWrapper contentWrapper, SpriteBatch spriteBatch)
        {
            base.Initialize(contentWrapper, spriteBatch);
            ticksToFire = 2000;
            bulletSpawnLocation = new Vector2(200, 200);
            //engine = new GameEngine(new EventManager(), new EntityManager(), contentWrapper);
            explosionAnimationFrames = new List<Rectangle>() {
                        new Rectangle(0, 0, 32, 32),
                        new Rectangle(32, 0, 32, 32),
                        new Rectangle(64, 0, 32, 32),
                        new Rectangle(0, 32, 32, 32),
                        new Rectangle(32, 32, 32, 32),
                        new Rectangle(64, 32, 32, 32),
                        new Rectangle(0, 64, 32, 32),
                        new Rectangle(32, 64, 32, 32),
                    };
            projectiveAnimationFrames = new List<Rectangle>() {
                            new Rectangle(0, 0, 32, 32),
                            new Rectangle(32, 0, 32, 32),
                            new Rectangle(64, 0, 32, 32),
                            new Rectangle(0, 32, 32, 32),
                            new Rectangle(32, 32, 32, 32),
                            new Rectangle(64, 32, 32, 32),
                            new Rectangle(0, 64, 32, 32),
                            new Rectangle(32, 64, 32, 32),
                        };

            explosionSounds = new List<SoundEffect>();
            engine.EventManager.SubscribeEvent(this, typeof(GameOverEvent));
            engine.EventManager.SubscribeEvent(this, typeof(OpenMenuEvent));
        }

        private void AddEntites()
        {
            mapId = engine.EntityManager.CreateEntity();
            engine.EntityManager.AddComponent(mapId, new PlaceableComponent()
            {
                Position = new Vector2(0, 0)
            });
            /**
            engine.EntityManager.AddComponent(mapId, new VisibleComponent()
            {
                Texture = mapToUse.Image,
                Source = new Rectangle(0, 0, mapToUse.Width, mapToUse.Height),
                Destination = new Rectangle(0, 0, mapToUse.Width, mapToUse.Height)
            });
            MapComponent mapComponent = new MapComponent()
            {
                Map = mapToUse
            };
            **/
            //engine.EntityManager.AddComponent(mapId, mapComponent);
        }

        /// <inheritdoc/>
        public override void LoadContent()
        {
            bulletTestExplosion = contentWrapper.Load<Texture2D>("Images/Effects/Explosion132x32-Sheet");
            bulletTest = contentWrapper.Load<Texture2D>("Images/Entities/BasicMunitionSprite");
            pixelTexture = contentWrapper.Load<Texture2D>("Images/Entities/Pixel");
            clouds = contentWrapper.Load<Texture2D>("Images/Entities/CloudSpritesheet");
            explosionSounds.Add(contentWrapper.Load<SoundEffect>("Sound/Effects/Explosion1"));
            explosionSounds.Add(contentWrapper.Load<SoundEffect>("Sound/Effects/Explosion2"));
            explosionSounds.Add(contentWrapper.Load<SoundEffect>("Sound/Effects/Explosion3"));
            explosionSounds.Add(contentWrapper.Load<SoundEffect>("Sound/Effects/Explosion4"));
            //defaultShader = contentWrapper.Load<Effect>("Shaders/Default");
            gameFont = contentWrapper.Load<SpriteFont>("gameFont");

        }

        /// <inheritdoc/>
        public override void SetActive()
        {
            base.SetActive();
            MediaPlayer.Stop();
            RandomSoundFactory soundFactory = new RandomSoundFactory(explosionSounds, randomizer);
            IGameObjectBuilder explosionBuilder = new BaseExplosionBuilder(bulletTestExplosion, explosionAnimationFrames, randomizer, soundFactory);
            explosionBuilder.Init(engine);
            List<IGameObjectBuilder> explosionBuilders = new List<IGameObjectBuilder>();
            explosionBuilders.Add(explosionBuilder);
            randomExplosionFactory = new RandomEntityBuilderFactory(explosionBuilders, randomizer);
            bulletBuilder = new BaseBulletBuilder(projectiveAnimationFrames, bulletTest, randomExplosionFactory);
            bulletBuilder.Init(engine);
            debriBuilder = new MapDebriBuilder(pixelTexture);
            debriBuilder.Init(engine);

            List<IGameObjectBuilder> cloudBuilders = new List<IGameObjectBuilder>();
            MapComponent map = engine.EntityManager.GetEntitiesWithComponent<MapComponent>()
                                                   .Select(id => engine.EntityManager.GetComponent<MapComponent>(id))
                                                   .FirstOrDefault();
            if (map == null)
            {
                gameOverState = new MainMenuState();
                return;
            }

            Rectangle cloudSpawnArea = new Rectangle(-50, 0, map.Width, (int)map.HighestPoint - 50);
            cloudBuilders.Add(new CloudBuilder(clouds, new List<Rectangle>() { new Rectangle(0, 0, 32, 16) }, randomizer, cloudSpawnArea));
            cloudBuilders.Add(new CloudBuilder(clouds, new List<Rectangle>() { new Rectangle(32, 0, 32, 16) }, randomizer, cloudSpawnArea));
            cloudBuilders.Add(new CloudBuilder(clouds, new List<Rectangle>() { new Rectangle(0, 16, 64, 32) }, randomizer, cloudSpawnArea));
            foreach (IGameObjectBuilder builder in cloudBuilders)
            {
                builder.Init(engine);
            }
            randomCloudFactory = new RandomEntityBuilderFactory(cloudBuilders, randomizer);

            AddEntites();
        }

        private void GenerateDebugEntity()
        {
            entityCounter = engine.EntityManager.CreateEntity(false);
            PlaceableComponent placeableComponent = engine.EntityManager.CreateComponent<PlaceableComponent>();
            placeableComponent.Position = Vector2.Zero;
            VisibleTextComponent VisibleTextComponent = engine.EntityManager.CreateComponent<VisibleTextComponent>();
            VisibleTextComponent.Font = gameFont;

            engine.EntityManager.AddComponent(entityCounter, placeableComponent);
            engine.EntityManager.AddComponent(entityCounter, VisibleTextComponent, true);
            debugIdGenerated = true;
        }

        private void RemoveDebugEntity()
        {
            engine.EntityManager.RemoveEntity(entityCounter);
            debugIdGenerated = false;
        }

        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {
            if (gameOverState != null)
            {
                gameStateManager.Replace(gameOverState);
            }

            int cloudCounter = engine.EntityManager.GetEntitiesWithComponent<CloudTag>().Count;
            if (cloudCounter < cloudsToSpawn || Keyboard.GetState().IsKeyDown(Keys.F6))
            {
                /**
                int leftClouds = cloudsToSpawn - cloudCounter;
                for (int i = 0; i < leftClouds; i++)
                {
                    uint cloudId = engine.EntityManager.CreateEntity();
                    foreach (IComponent component in randomCloudFactory.GetNewObject())
                    {
                        engine.EntityManager.AddComponent(cloudId, component);
                    }
                }
                **/
            }

            if (ticksToFire > 0)
            {
                ticksToFire--;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.F2) && !previousState.IsKeyDown(Keys.F2) && debugOn)
            {
                /**
                uint debriId = engine.EntityManager.CreateEntity(false);
                foreach(IComponent component in debriBuilder.BuildGameComponents(Color.Red))
                {
                    engine.EntityManager.AddComponent(debriId, component);

                    if (component is PlaceableComponent placeable)
                    {
                        placeable.Position += Mouse.GetState().Position.ToVector2();
                    }
                }
                **/
                uint projectileId = engine.EntityManager.CreateEntity(false);
                foreach (IComponent component in bulletBuilder.BuildGameComponents())
                {
                    if (component is MoveableComponent moveable)
                    {
                        moveable.Velocity = new Vector2(randomizer.GetNewNumber(10, 15), 0);
                    }
                    if (component is PlaceableComponent placeable)
                    {
                        placeable.Position += bulletSpawnLocation;
                    }
                    engine.EntityManager.AddComponent(projectileId, component);
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.F1) && !previousState.IsKeyDown(Keys.F1))
            {
                if (debugOn)
                {
                    debugOn = false;
                    RemoveDebugEntity();
                }
                else
                {
                    debugOn = true;
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.F5) && !previousState.IsKeyDown(Keys.F5) && debugOn)
            {
                IState gameLoading = new GameLoadingScreen(
                    new MidpointDisplacementGenerator(
                        viewportAdapter.VirtualWidth / 4,
                        0.5f,
                        new SystemRandomizer()
                        ), gameSettings);
                gameStateManager.Replace(gameLoading);
            }

            if (Mouse.GetState().LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released && debugOn)
            {
                uint explosion = engine.EntityManager.CreateEntity();

                foreach (IComponent component in randomExplosionFactory.GetNewObject())
                {
                    Circle circle = null;
                    if (component is PlaceableComponent)
                    {
                        PlaceableComponent placeableComponent = (PlaceableComponent)component;
                        placeableComponent.Position = mouseWrapper.GetMouseVectorPosition();
                        circle = new Circle(mouseWrapper.GetMouseVectorPosition(), 16);
                    }
                    engine.EntityManager.AddComponent(explosion, component);
                    if (circle != null)
                    {
                        DamageComponent damage = engine.EntityManager.CreateComponent<DamageComponent>();
                        damage.DamagingDone = false;
                        damage.CenterDamageValue = 100;
                        damage.DamageArea = circle;
                        damage.EffectFactory = randomExplosionFactory;
                        engine.EntityManager.AddComponent(explosion, damage);

                        DamageTerrainEvent damageTerrainEvent = engine.EventManager.CreateEvent<DamageTerrainEvent>();
                        damageTerrainEvent.DamageArea = circle;

                        engine.EventManager.FireEvent<DamageTerrainEvent>(this, damageTerrainEvent);
                        MapCollisionEvent mapCollisionEvent = engine.EventManager.CreateEvent<MapCollisionEvent>();
                        mapCollisionEvent.EntityId = explosion;
                        mapCollisionEvent.Position = circle.Center;
                        engine.EventManager.FireEvent<MapCollisionEvent>(this, mapCollisionEvent);
                    }
                }
            }

            if (debugOn)
            {
                if (!debugIdGenerated)
                {
                    GenerateDebugEntity();
                    debugIdGenerated = true;
                }
                VisibleTextComponent entityCounterText = engine.EntityManager.GetComponent<VisibleTextComponent>(entityCounter);
                if (entityCounterText != null)
                {
                    entityCounterText.Text = "Fps: " + fps;
                    entityCounterText.Text += "\nUpdate ms: " + gameTime.ElapsedGameTime.TotalMilliseconds;
                    entityCounterText.Text += "\nEntities: " + engine.GetEntityCount();
                    entityCounterText.Text += "\nComponents: " + engine.GetComponentCount();
                    entityCounterText.Text += "\nUsed Components: " + engine.GetUsedComponentCount();
                    entityCounterText.Text += "\nSystems: " + engine.GetSystemCount();
                    entityCounterText.Text += engine.GetSystemWatchedEntites();
                }
            }
            engine.Update(gameTime);
            previousState = Keyboard.GetState();
            previousMouseState = Mouse.GetState();
            if (openMenu)
            {
                openMenu = false;
                gameStateManager.Add(new EscMenuScreen());
            }
        }

        /// <inheritdoc/>
        public override void Draw(GameTime gameTime)
        {
            if (debugOn)
            {
                fps = (float)Math.Round(1 / gameTime.ElapsedGameTime.TotalSeconds);
            }
            engine.Draw(gameTime);
        }

        /// <inheritdoc/>
        public override void Destruct()
        {
            engine.Clear();
            if (MediaPlayer.State == MediaState.Playing)
            {
                MediaPlayer.Stop();
            }
        }

        public void EventNotification(object sender, IGameEvent eventArgs)
        {
            if (eventArgs is GameOverEvent)
            {
                gameOverState = new MainMenuState();
            }

            if (eventArgs is OpenMenuEvent)
            {
                openMenu = true;
            }
        }
    }
}
