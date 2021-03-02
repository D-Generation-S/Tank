using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Tank.Builders;
using Tank.Components;
using Tank.Components.Rendering;
using Tank.DataStructure.Geometrics;
using Tank.EntityComponentSystem.Manager;
using Tank.Events.PhysicBased;
using Tank.Events.TerrainEvents;
using Tank.Factories;
using Tank.GameStates.Data;
using Tank.Interfaces.Builders;
using Tank.Interfaces.EntityComponentSystem;
using Tank.Interfaces.EntityComponentSystem.Manager;
using Tank.Interfaces.MapGenerators;
using Tank.Map.Generators;
using Tank.Randomizer;
using Tank.Systems;
using Tank.Wrapper;

namespace Tank.GameStates.States
{
    class GameState : BaseAbstractState
    {
        private int ticksToFire;

        private KeyboardState previousState;
        private MouseState previousMouseState;

        private RandomExplosionFactory randomExplosionFactory;

        private BaseBulletBuilder bulletBuilder;
        private MapDebriBuilder debriBuilder;

        private List<IGameObjectBuilder> explosionBuilders;

        private List<Rectangle> projectiveAnimationFrames;
        private List<Rectangle> explosionAnimationFrames;

        private List<SoundEffect> explosionSounds;
        private Effect defaultShader;


        Vector2 bulletSpawnLocation;

        Texture2D bulletTestExplosion;
        Texture2D bulletTest;
        private Texture2D pixelTexture;
        private SpriteFont gameFont;

        IGameEngine engine;
        private readonly IMap mapToUse;
        private readonly GameSettings gameSettings;
        private readonly SystemRandomizer randomizer;
        
        private uint mapId;
        private uint entityCounter;
        private bool debugOn;
        private bool debugIdGenerated;

        private float fps;

        public GameState(IMap mapToUse, GameSettings gameSettings)
        {
            this.mapToUse = mapToUse;
            this.gameSettings = gameSettings;
            randomizer = new SystemRandomizer();
            randomizer.Initzialize(100);
            debugOn = gameSettings.IsDebug;
            debugIdGenerated = false;
        }

        public override void Initialize(ContentWrapper contentWrapper, SpriteBatch spriteBatch)
        {
            base.Initialize(contentWrapper, spriteBatch);
            ticksToFire = 2000;
            bulletSpawnLocation = new Vector2(200, 200);
            engine = new GameEngine(new EventManager(), new EntityManager(), contentWrapper);
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

            explosionBuilders = new List<IGameObjectBuilder>();
            explosionSounds = new List<SoundEffect>();
        }

        private void AddEngineSystems()
        {
            int screenWidth = TankGame.PublicGraphicsDevice.Viewport.Width;
            int screenHeight = TankGame.PublicGraphicsDevice.Viewport.Height;

            engine.AddSystem(new BindingSystem());
            engine.AddSystem(new MapSculptingSystem());
            engine.AddSystem(new ForceSystem(new VectorRectangle(0, 0, screenWidth, screenHeight)));
            engine.AddSystem(new PhysicSystem(new Rectangle(0, 0, screenWidth, screenHeight), gameSettings.Gravity, gameSettings.Wind));
            engine.AddSystem(new AnimationSystem());
            engine.AddSystem(new DamageSystem());
            engine.AddSystem(new SoundEffectSystem());
            engine.AddSystem(new RenderSystem(
                 spriteBatch,
                 TankGame.PublicGraphicsDevice,
                 defaultShader
             ));
            engine.AddSystem(new GameLogicSystem(gameSettings.PlayerCount, mapToUse));
        }

        private void AddEntites()
        {
            mapId = engine.EntityManager.CreateEntity();
            engine.EntityManager.AddComponent(mapId, new PlaceableComponent()
            {
                Position = new Vector2(0, 0)
            });

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
            engine.EntityManager.AddComponent(mapId, mapComponent);
        }

        public override void LoadContent()
        {
            bulletTestExplosion = contentWrapper.Content.Load<Texture2D>("Images/Effects/Explosion132x32-Sheet");
            bulletTest = contentWrapper.Content.Load<Texture2D>("Images/Entities/BasicMunitionSprite");
            pixelTexture = contentWrapper.Content.Load<Texture2D>("Images/Entities/Pixel");
            explosionSounds.Add(contentWrapper.Content.Load<SoundEffect>("Sound/Effects/Explosion1"));
            explosionSounds.Add(contentWrapper.Content.Load<SoundEffect>("Sound/Effects/Explosion2"));
            explosionSounds.Add(contentWrapper.Content.Load<SoundEffect>("Sound/Effects/Explosion3"));
            explosionSounds.Add(contentWrapper.Content.Load<SoundEffect>("Sound/Effects/Explosion4"));
            defaultShader = contentWrapper.Content.Load<Effect>("Shaders/Default");
            gameFont = contentWrapper.Content.Load<SpriteFont>("gameFont");
        }

        public override void SetActive()
        {
            RandomSoundFactory soundFactory = new RandomSoundFactory(explosionSounds, randomizer);
            IGameObjectBuilder explosionBuilder = new BaseExplosionBuilder(bulletTestExplosion, explosionAnimationFrames, soundFactory);
            explosionBuilder.Init(engine);
            explosionBuilders.Add(explosionBuilder);
            randomExplosionFactory = new RandomExplosionFactory(explosionBuilders, randomizer);
            bulletBuilder = new BaseBulletBuilder(projectiveAnimationFrames, bulletTest, randomExplosionFactory);
            bulletBuilder.Init(engine);
            debriBuilder = new MapDebriBuilder(pixelTexture);
            debriBuilder.Init(engine);

            AddEngineSystems();
            AddEntites();
        }

        private void GenerateDebugEntity()
        {
            entityCounter = engine.EntityManager.CreateEntity(false);
            engine.EntityManager.AddComponent(entityCounter, new PlaceableComponent()
            {
                Position = Vector2.Zero
            });

            engine.EntityManager.AddComponent(entityCounter, new VisibleTextComponent()
            {
                Text = "",
                Color = Color.White,
                Font = gameFont,
                Scale = 1f
            });
            debugIdGenerated = true;
        }

        private void RemoveDebugEntity()
        {
            engine.EntityManager.RemoveEntity(entityCounter);
            debugIdGenerated = false;
        }

        public override void Update(GameTime gameTime)
        {
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
                }
            }
            engine.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (debugOn)
            {
                fps = (float)Math.Round(1 / gameTime.ElapsedGameTime.TotalSeconds);
            }
            if (ticksToFire > 0)
            {
                ticksToFire--;
            }
            if (ticksToFire > 0 || Keyboard.GetState().IsKeyDown(Keys.F2) && !previousState.IsKeyDown(Keys.F2))
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
                } else
                {
                    debugOn = true;
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.F5) && !previousState.IsKeyDown(Keys.F5))
            {
                IState gameLoading = new GameLoadingScreen(new MidpointDisplacementGenerator(TankGame.PublicGraphicsDevice, 900 / 4, 0.5f, new SystemRandomizer()), gameSettings);
                gameStateManager.Replace(gameLoading);
            }

            if (Mouse.GetState().LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
            {
                uint exposion = engine.EntityManager.CreateEntity();

                foreach (IComponent component in randomExplosionFactory.GetGameObjects())
                {
                    Circle circle = null;
                    if (component is PlaceableComponent)
                    {
                        PlaceableComponent placeableComponent = (PlaceableComponent)component;
                        placeableComponent.Position = Mouse.GetState().Position.ToVector2();
                        placeableComponent.Position -= new Vector2(32 / 2, 32 / 2);
                        circle = new Circle(Mouse.GetState().Position.ToVector2(), 16);
                    }
                    engine.EntityManager.AddComponent(exposion, component);
                    if (circle != null)
                    {
                        DamageComponent damage = new DamageComponent()
                        {
                            DamagingDone = false,
                            CenterDamageValue = 100,
                            DamageArea = circle,
                            EffectFactory = randomExplosionFactory

                        };
                        engine.EntityManager.AddComponent(exposion, damage);
                        engine.EventManager.FireEvent<DamageTerrainEvent>(this, new DamageTerrainEvent(circle));
                        engine.EventManager.FireEvent<MapCollisionEvent>(this, new MapCollisionEvent(exposion, circle.Center));
                    }
                }
            }

            previousState = Keyboard.GetState();
            previousMouseState = Mouse.GetState();
            engine.Draw(gameTime);
        }

        public override void Destruct()
        {
            engine.Clear();
        }
    }
}
