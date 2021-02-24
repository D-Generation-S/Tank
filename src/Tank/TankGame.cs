using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tank.Builders;
using Tank.Components;
using Tank.Components.Rendering;
using Tank.DataStructure;
using Tank.DataStructure.Spritesheet;
using Tank.EntityComponentSystem.Manager;
using Tank.Events.PhysicBased;
using Tank.Events.TerrainEvents;
using Tank.Factories;
using Tank.Interfaces.Builders;
using Tank.Interfaces.EntityComponentSystem;
using Tank.Interfaces.EntityComponentSystem.Manager;
using Tank.Interfaces.MapGenerators;
using Tank.Interfaces.Randomizer;
using Tank.Map.Generators;
using Tank.Map.Textureizer;
using Tank.Randomizer;
using Tank.Systems;
using Tank.Utils;
using Tank.Wrapper;

namespace Tank
{
    /// <summary>
    /// Main entry point of the game
    /// </summary>
    public class TankGame : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        KeyboardState previousState;
        MouseState previousMouseState;
        public static GraphicsDevice PublicGraphicsDevice;
        public static ContentManager PublicContentManager;

        IGameEngine engine;
        IMapGenerator mapGenerator;
        IRandomizer randomizer;

        Texture2D bulletTestExplosion;
        Texture2D bulletTest;
        private List<SoundEffect> soundEffectsTest;
        List<Rectangle> explosionAnimationFrames;

        uint entityCounter;
        int ticksToFire = 2000;
        private RandomExplosionFactory randomExplosionFactory;
        private List<IGameObjectBuilder> explosionBuilders;
        private List<Rectangle> projectiveAnimationFrames;
        private BaseBulletBuilder bulletBuilder;
        Vector2 bulletSpawnLocation;


        public TankGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            PublicGraphicsDevice = GraphicsDevice;
            PublicContentManager = Content;
            InitResolution(1440, 900);
            bulletSpawnLocation = new Vector2(200, 200);
            engine = new GameEngine(new EventManager(), new EntityManager(), new ContentWrapper(Content));
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

            bulletTestExplosion = Content.Load<Texture2D>("Images/Effects/Explosion132x32-Sheet");
            bulletTest = Content.Load<Texture2D>("Images/Entities/BasicMunitionSprite");

            soundEffectsTest = new List<SoundEffect>();
            soundEffectsTest.Add(Content.Load<SoundEffect>("Sound/Effects/Explosion1"));
            soundEffectsTest.Add(Content.Load<SoundEffect>("Sound/Effects/Explosion2"));
            soundEffectsTest.Add(Content.Load<SoundEffect>("Sound/Effects/Explosion3"));
            soundEffectsTest.Add(Content.Load<SoundEffect>("Sound/Effects/Explosion4"));

            randomizer = new SystemRandomizer();
            randomizer.Initzialize(100);
            explosionBuilders = new List<IGameObjectBuilder>();
            RandomSoundFactory soundFactory = new RandomSoundFactory(soundEffectsTest, randomizer);
            IGameObjectBuilder explosionBuilder = new BaseExplosionBuilder(bulletTestExplosion, explosionAnimationFrames, soundFactory);
            explosionBuilder.Init(engine);
            explosionBuilders.Add(explosionBuilder);
            randomExplosionFactory = new RandomExplosionFactory(explosionBuilders, randomizer);
            bulletBuilder = new BaseBulletBuilder(projectiveAnimationFrames, bulletTest, randomExplosionFactory);
            bulletBuilder.Init(engine);

            base.Initialize();

            engine.AddSystem(new BindingSystem());
            engine.AddSystem(new ForceSystem());
            engine.AddSystem(new PhysicSystem(new Rectangle(0, 0, 1920, 1080), 0.098f, 0.3f));

            engine.AddSystem(new RenderSystem(spriteBatch));
            engine.AddSystem(new AnimationSystem());
            engine.AddSystem(new DamageSystem());
            engine.AddSystem(new MapDestructionSystem());
            engine.AddSystem(new SoundEffectSystem());

            uint mapId = engine.EntityManager.CreateEntity();
            engine.EntityManager.AddComponent(mapId, new PlaceableComponent()
            {
                Position = new Vector2(0, 0)
            });
            engine.EntityManager.AddComponent(mapId, new VisibleComponent());

            mapGenerator = new MidpointDisplacementGenerator(GraphicsDevice, 900 / 4, 0.5f, randomizer);
            SpriteSheet spriteSheet = new SpriteSheet(
            Content.Load<Texture2D>("Images/Textures/MoistContinentalSpritesheet"),
            new Position(32, 32),
            0
            );
            spriteSheet.SetSpriteSheetPattern(new List<SpriteSheetPattern>()
            {
                new SpriteSheetPattern("dirt", new Position(0,1)),
                new SpriteSheetPattern("stone", new Position(1,1))
            });

            FlattenArray<Color> stone = spriteSheet.GetTextureByName("stone");
            Task<IMap> mapCreatingTask = mapGenerator.AsyncGenerateNewMap(
                new Position(
                    1440,
                    900
                ),
                new DefaultTextureizer(spriteSheet)
            );
            mapCreatingTask.ContinueWith((antecedent) =>
            {
                MapComponent mapComponent = new MapComponent()
                {
                    Map = antecedent.Result
                };
                engine.EntityManager.AddComponent(mapId, mapComponent);
                engine.AddSystem(new GameLogicSystem(4, antecedent.Result));

                VisibleComponent visibleComponent = engine.EntityManager.GetComponent<VisibleComponent>(mapId);
                IMap map = mapComponent.Map;
                visibleComponent.Texture = map.Image;
                visibleComponent.Source = new Rectangle(0, 0, map.Width, map.Height);
                visibleComponent.Destination = visibleComponent.Source;


                entityCounter = engine.EntityManager.CreateEntity(false);
                engine.EntityManager.AddComponent(entityCounter, new PlaceableComponent()
                {
                    Position = Vector2.Zero
                });

                engine.EntityManager.AddComponent(entityCounter, new VisibleTextComponent()
                {
                    Text = "",
                    Color = Color.White,
                    Font = Content.Load<SpriteFont>("gameFont"),
                    Scale = 2f
                });
            });

            IsMouseVisible = true;
        }

        private void InitResolution(int windowWidth, int windowHeight)
        {
            graphics.PreferredBackBufferWidth = windowWidth;
            graphics.PreferredBackBufferHeight = windowHeight;

            graphics.IsFullScreen = false;
            graphics.ApplyChanges();

        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Content.Load<Texture2D>("Images/Entities/BasicMunitionSprite");
        }

        protected override void Update(GameTime gameTime)
        {
            if (IsActive)
            {
                VisibleTextComponent entityCounterText = engine.EntityManager.GetComponent<VisibleTextComponent>(entityCounter);
                if (entityCounterText != null)
                {
                    entityCounterText.Text = "Fps: " + Math.Round(1 / gameTime.ElapsedGameTime.TotalSeconds);
                    entityCounterText.Text += "\nEntities: " + engine.GetEntityCount();
                    entityCounterText.Text += "\nComponents: " + engine.GetComponentCount();
                    entityCounterText.Text += "\nUsed Components: " + engine.GetUsedComponentCount();
                    entityCounterText.Text += "\nSystems: " + engine.GetSystemCount();
                }


                engine.Update(gameTime);
                ticksToFire--;
                if (ticksToFire > 0 || Keyboard.GetState().IsKeyDown(Keys.F2) && !previousState.IsKeyDown(Keys.F2))
                {
                    uint projectileId = engine.EntityManager.CreateEntity(false);
                    foreach(IComponent component in bulletBuilder.BuildGameComponents())
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
                                EffectFactory = randomExplosionFactory,
                                PushbackForce = 9000

                            };
                            engine.EntityManager.AddComponent(exposion, damage);
                            engine.EventManager.FireEvent<DamageTerrainEvent>(this, new DamageTerrainEvent(circle));
                            engine.EventManager.FireEvent<MapCollisionEvent>(this, new MapCollisionEvent(exposion, circle.Center));
                        }
                    }
                }
                base.Update(gameTime);

                previousState = Keyboard.GetState();
                previousMouseState = Mouse.GetState();
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, null);
            GraphicsDevice.Clear(Color.CornflowerBlue);
            engine.Draw(gameTime);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}