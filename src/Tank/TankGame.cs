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
using Tank.Components.Forces;
using Tank.Components.Tags;
using Tank.DataStructure;
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

        bool allowBulletSpawn;

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

            base.Initialize();
            randomizer = new SystemRandomizer();
            randomizer.Initzialize(100);

            engine = new GameEngine(new EventManager(), new EntityManager(), new ContentWrapper(Content));
            engine.AddSystem(new BindingSystem());
            engine.AddSystem(new ForceSystem());
            engine.AddSystem(new PhysicSystem(new Rectangle(0, 0, 1920, 1080), 0.098f, 0.3f));

            engine.AddSystem(new RenderSystem(spriteBatch));
            engine.AddSystem(new AnimationSystem());
            //engine.AddSystem(new MapColliderSystem());
            engine.AddSystem(new DamageSystem());
            engine.AddSystem(new MapDestructionSystem());
            engine.AddSystem(new SoundEffectSystem());

            uint mapId = engine.EntityManager.CreateEntity();
            engine.EntityManager.AddComponent(mapId, new PlaceableComponent());
            engine.EntityManager.AddComponent(mapId, new VisibleComponent());

            bulletTestExplosion = Content.Load<Texture2D>("Images/Effects/Explosion132x32-Sheet");
            bulletTest = Content.Load<Texture2D>("Images/Entities/BasicMunitionSprite");
            soundEffectsTest = new List<SoundEffect>();
            soundEffectsTest.Add(Content.Load<SoundEffect>("Sound/Effects/Explosion1"));
            soundEffectsTest.Add(Content.Load<SoundEffect>("Sound/Effects/Explosion2"));
            soundEffectsTest.Add(Content.Load<SoundEffect>("Sound/Effects/Explosion3"));
            soundEffectsTest.Add(Content.Load<SoundEffect>("Sound/Effects/Explosion4"));
            mapGenerator = new MidpointDisplacementGenerator(GraphicsDevice, 900 / 4, 0.5f, randomizer);
            SpriteSheet spriteSheet = new SpriteSheet(
            Content.Load<Texture2D>("Images/Textures/DefaultMapSprites"),
            new Position(64, 64),
            0
            );
            Task<IMap> mapCreatingTask = mapGenerator.AsyncGenerateNewMap(
                new Position(
                    1440,
                    900
                ),
                new DefaultTextureizer(spriteSheet)
            );
            mapCreatingTask.ContinueWith((antecedent) =>
            {
                MapComponent mapComponent = new MapComponent(antecedent.Result);
                engine.EntityManager.AddComponent(mapId, mapComponent);
                engine.AddSystem(new GameLogicSystem(4, antecedent.Result));

                VisibleComponent visibleComponent = engine.EntityManager.GetComponent<VisibleComponent>(mapId);
                IMap map = mapComponent.Map;
                visibleComponent.Texture = map.Image;
                visibleComponent.Source = new Rectangle(0, 0, map.Width, map.Height);
                visibleComponent.Destination = visibleComponent.Source;
                allowBulletSpawn = true;
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


        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (IsActive)
            {
                engine.Update(gameTime);
                if (Keyboard.GetState().IsKeyDown(Keys.F2) && !previousState.IsKeyDown(Keys.F2))
                {
                        uint projectileId = engine.EntityManager.CreateEntity();

                        List<Rectangle> animationFrames = new List<Rectangle>() {
                        new Rectangle(0, 0, 32, 32),
                        new Rectangle(32, 0, 32, 32),
                        new Rectangle(64, 0, 32, 32),
                        new Rectangle(0, 32, 32, 32),
                        new Rectangle(32, 32, 32, 32),
                        new Rectangle(64, 32, 32, 32),
                        new Rectangle(0, 64, 32, 32),
                        new Rectangle(32, 64, 32, 32),
                    };
                        RandomSoundFactory soundFactory = new RandomSoundFactory(soundEffectsTest, randomizer);
                        List<IGameObjectBuilder> explosionBuilders = new List<IGameObjectBuilder>();
                        explosionBuilders.Add(new BaseExplosionBuilder(bulletTestExplosion, animationFrames, soundFactory));
                        RandomExplosionFactory randomExplosionFactory = new RandomExplosionFactory(explosionBuilders, randomizer);

                        engine.EntityManager.AddComponent(projectileId, new PlaceableComponent() { Position = new Vector2(200, 200) });
                        VisibleComponent visible = new VisibleComponent();
                        visible.Texture = bulletTest;
                        visible.Source = new Rectangle(0, 0, 32, 32);
                        visible.Destination = new Rectangle(0, 0, 32, 32);
                        engine.EntityManager.AddComponent(projectileId, visible);
                        engine.EntityManager.AddComponent(projectileId, new MapColliderTag());
                        List<Rectangle> spriteSources = new List<Rectangle>() {
                        new Rectangle(0, 0, 32, 32),
                        new Rectangle(32, 0, 32, 32),
                        new Rectangle(64, 0, 32, 32),
                        new Rectangle(0, 32, 32, 32),
                        new Rectangle(32, 32, 32, 32),
                        new Rectangle(64, 32, 32, 32),
                        new Rectangle(0, 64, 32, 32),
                        new Rectangle(32, 64, 32, 32),
                    };
                        engine.EntityManager.AddComponent(projectileId, new AnimationComponent(0.02f, spriteSources)
                        {
                            Name = "Idle",
                            Active = true,
                            Loop = true,
                            PingPong = true
                        });

                        engine.EntityManager.AddComponent(projectileId, new ColliderComponent(true, true)
                        {
                            Collider = new Rectangle(0, 0, 32, 32),
                        });
                        engine.EntityManager.AddComponent(projectileId, new DamageComponent(true, 1, new Circle(0, 0, 16), randomExplosionFactory)
                        {
                        });
                        engine.EntityManager.AddComponent(projectileId, new MoveableComponent()
                        {
                            Velocity = new Vector2((new Random()).Next(5, 30), 0),
                            PhysicRotate = true,
                            Mass = 7
                        });
                }

                if (Mouse.GetState().LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
                {
                    uint exposion = engine.EntityManager.CreateEntity();
                    List<Rectangle> animationFrames = new List<Rectangle>() {
                        new Rectangle(0, 0, 32, 32),
                        new Rectangle(32, 0, 32, 32),
                        new Rectangle(64, 0, 32, 32),
                        new Rectangle(0, 32, 32, 32),
                        new Rectangle(32, 32, 32, 32),
                        new Rectangle(64, 32, 32, 32),
                        new Rectangle(0, 64, 32, 32),
                        new Rectangle(32, 64, 32, 32),
                    };
                    RandomSoundFactory soundFactory = new RandomSoundFactory(soundEffectsTest, randomizer);
                    List<IGameObjectBuilder> explosionBuilders = new List<IGameObjectBuilder>();
                    explosionBuilders.Add(new BaseExplosionBuilder(bulletTestExplosion, animationFrames, soundFactory));
                    RandomExplosionFactory randomExplosionFactory = new RandomExplosionFactory(explosionBuilders, randomizer);

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
                            DamageComponent damage = new DamageComponent(false, 100, circle, randomExplosionFactory, 9000);
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