﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Tank.Components;
using Tank.src.Builders;
using Tank.src.Code.MapGenerators.Generatos;
using Tank.src.Code.Textureizer;
using Tank.src.DataStructure;
using Tank.src.EntityComponentSystem.Manager;
using Tank.src.Events.EntityBased;
using Tank.src.Events.PhysicBased;
using Tank.src.Events.TerrainEvents;
using Tank.src.Factories;
using Tank.src.Interfaces.Builders;
using Tank.src.Interfaces.EntityComponentSystem.Manager;
using Tank.src.Interfaces.MapGenerators;
using Tank.src.Interfaces.Randomizer;
using Tank.src.Randomizer;
using Tank.src.Systems;
using Tank.Systems;

namespace Tank
{
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

            engine = new GameEngine(new EventManager(), new EntityManager(), new src.Wrapper.ContentWrapper(Content));
            engine.AddSystem(new RenderSystem(spriteBatch));
            engine.AddSystem(new BindingSystem());
            engine.AddSystem(new AnimationSystem());
            engine.AddSystem(new PhysicSystem(new Rectangle(0, 0, 1920, 1080), 9.8f, 30));
            //engine.AddSystem(new MapColliderSystem());
            engine.AddSystem(new DamageSystem());
            engine.AddSystem(new MapDestructionSystem());
            engine.AddSystem(new SoundEffectSystem());

            uint mapId = engine.EntityManager.CreateEntity();
            engine.EntityManager.AddComponent(mapId, new PlaceableComponent());
            engine.EntityManager.AddComponent(mapId, new VisibleComponent());

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
            mapCreatingTask.ContinueWith((antecedent) => {
                MapComponent mapComponent = new MapComponent(antecedent.Result);
                engine.EntityManager.AddComponent(mapId, mapComponent);
                engine.AddSystem(new GameLogicSystem(1, 1440));

                VisibleComponent visibleComponent = engine.EntityManager.GetComponent<VisibleComponent>(mapId);
                IMap map = mapComponent.Map;
                visibleComponent.Texture = map.Image;
                visibleComponent.Source = new Rectangle(0, 0, map.Width, map.Height);
                visibleComponent.Destination = visibleComponent.Source;
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
                    List<SoundEffect> soundEffects = new List<SoundEffect>();
                    soundEffects.Add(Content.Load<SoundEffect>("Sound/Effects/Explosion1"));
                    soundEffects.Add(Content.Load<SoundEffect>("Sound/Effects/Explosion2"));
                    soundEffects.Add(Content.Load<SoundEffect>("Sound/Effects/Explosion3"));
                    soundEffects.Add(Content.Load<SoundEffect>("Sound/Effects/Explosion4"));
                    RandomSoundFactory soundFactory = new RandomSoundFactory(soundEffects, randomizer);
                    List<IGameObjectBuilder> explosionBuilders = new List<IGameObjectBuilder>();
                    explosionBuilders.Add(new BaseExplosionBuilder(Content.Load<Texture2D>("Images/Effects/Explosion132x32-Sheet"), animationFrames, soundFactory));
                    RandomExplosionFactory randomExplosionFactory = new RandomExplosionFactory(explosionBuilders, randomizer);

                    engine.EntityManager.AddComponent(projectileId, new PlaceableComponent() { Position = new Vector2(200, 200) });
                    VisibleComponent visible = new VisibleComponent();
                    visible.Texture = Content.Load<Texture2D>("Images/Entities/BasicMunitionSprite");
                    visible.Source = new Rectangle(0, 0, 32, 32);
                    visible.Destination = new Rectangle(0, 0, 32, 32);
                    engine.EntityManager.AddComponent(projectileId, visible);
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

                    engine.EntityManager.AddComponent(projectileId, new ColliderComponent()
                    {
                        Collider = new Rectangle(0, 0, 32, 32)
                    });
                    engine.EntityManager.AddComponent(projectileId, new DamageComponent(true, 1, new src.DataStructure.Circle(0, 0, 16), randomExplosionFactory)
                    {
                    });
                    engine.EntityManager.AddComponent(projectileId, new MoveableComponent()
                    {
                        Velocity = new Vector2((new Random()).Next(10, 20), 0),
                        PhysicRotate = true,
                        Mass = 30
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
                    List<SoundEffect> soundEffects = new List<SoundEffect>();
                    soundEffects.Add(Content.Load<SoundEffect>("Sound/Effects/Explosion1"));
                    soundEffects.Add(Content.Load<SoundEffect>("Sound/Effects/Explosion2"));
                    soundEffects.Add(Content.Load<SoundEffect>("Sound/Effects/Explosion3"));
                    soundEffects.Add(Content.Load<SoundEffect>("Sound/Effects/Explosion4"));
                    RandomSoundFactory soundFactory = new RandomSoundFactory(soundEffects, randomizer);
                    List<IGameObjectBuilder> explosionBuilders = new List<IGameObjectBuilder>();
                    Texture2D explosion = Content.Load<Texture2D>("Images/Effects/Explosion132x32-Sheet");
                    explosionBuilders.Add(new BaseExplosionBuilder(explosion, animationFrames, soundFactory));
                    RandomExplosionFactory randomExplosionFactory = new RandomExplosionFactory(explosionBuilders, randomizer);

                    foreach(src.Interfaces.EntityComponentSystem.IComponent component in randomExplosionFactory.GetGameObjects())
                    {
                        Circle circle = null;
                        if (component is PlaceableComponent)
                        {
                            PlaceableComponent placeableComponent = (PlaceableComponent)component;
                            placeableComponent.Position = Mouse.GetState().Position.ToVector2();
                            placeableComponent.Position -= new Vector2(32 / 2, 32 / 2);
                            circle = new Circle(Mouse.GetState().Position.ToVector2(), 64);
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