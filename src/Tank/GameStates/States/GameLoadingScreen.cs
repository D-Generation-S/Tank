using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tank.Builders;
using Tank.Components;
using Tank.Components.DataLookup;
using Tank.Components.GameObject;
using Tank.Components.Input;
using Tank.Components.Rendering;
using Tank.Components.Tags;
using Tank.DataManagement;
using Tank.DataManagement.Data;
using Tank.DataManagement.Loader;
using Tank.Factories;
using Tank.GameStates.Data;
using Tank.Interfaces.Builders;
using Tank.Interfaces.MapGenerators;
using Tank.Map.Textureizer;
using Tank.Register;
using Tank.Systems;
using Tank.Utils;
using TankEngine.DataProvider.Loader;
using TankEngine.DataStructures.Geometrics;
using TankEngine.DataStructures.Spritesheet;
using TankEngine.EntityComponentSystem;
using TankEngine.EntityComponentSystem.Events;
using TankEngine.EntityComponentSystem.Manager;
using TankEngine.GameStates.States;
using TankEngine.Music;
using TankEngine.Randomizer;
using TankEngine.Wrapper;

namespace Tank.GameStates.States
{
    /// <summary>
    /// The game loading screent
    /// </summary>
    class GameLoadingScreen : BaseAbstractState
    {
        /// <summary>
        /// The map generator to use
        /// </summary>
        private readonly IMapGenerator mapGenerator;

        /// <summary>
        /// The game settings to use
        /// </summary>
        private readonly GameSettings gameSettings;

        /// <summary>
        /// The data loader to use
        /// </summary>
        private readonly IDataLoader<SpritesheetData> dataLoader;

        /// <summary>
        /// The manager used to load the sprites
        /// </summary>
        private DataManager<SpritesheetData> spriteSetManager;

        /// <summary>
        /// The spritesheet to use for the map
        /// </summary>
        private SpriteSheet spritesheetToUse;

        /// <summary>
        /// Is loading complete
        /// </summary>
        private bool loadingComplete;

        /// <summary>
        /// The default shader to use
        /// </summary>
        private Effect defaultShader;

        /// <summary>
        /// The game font to use
        /// </summary>
        private SpriteFont gameFont;

        /// <summary>
        /// The game engine to use
        /// </summary>
        private IGameEngine engine;

        /// <summary>
        /// The underlaying entity manager
        /// </summary>
        private IEntityManager entityManager => engine.EntityManager;

        /// <summary>
        /// Sound to use for explosions
        /// </summary>
        private List<SoundEffect> explosionSounds;

        /// <summary>
        /// Standard texture for the standard shell explosion
        /// </summary>
        private Texture2D standardShellExplosion;

        /// <summary>
        /// The standard shell texture
        /// </summary>
        private Texture2D standardShellTexture;

        /// <summary>
        /// The standard shell animation
        /// </summary>
        private List<Rectangle> standardShellAnimation;

        /// <summary>
        /// The standard shell explosion animation
        /// </summary>
        private List<Rectangle> standardShellExplosionAnimation;

        /// <summary>
        /// The sprite sheet to use for the heathbar
        /// </summary>
        private SpriteSheet healthBarSprite;

        /// <summary>
        /// The sprite to use for the power bar
        /// </summary>
        private SpriteSheet powerBarSprite;

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="mapGenerator">The map generating algorihm to use</param>
        /// <param name="gameSettings">The game settings to use</param>
        public GameLoadingScreen(IMapGenerator mapGenerator, GameSettings gameSettings)
            : this(mapGenerator, gameSettings, new JsonGameDataLoader<SpritesheetData>("Spritesheets"))
        {
        }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="mapGenerator">The map generating algorihm to use</param>
        /// <param name="gameSettings">The game settings to use</param>
        /// <param name="dataLoader">The data loader to use</param>
        public GameLoadingScreen(IMapGenerator mapGenerator, GameSettings gameSettings, IDataLoader<SpritesheetData> dataLoader)
        {
            this.mapGenerator = mapGenerator;
            this.gameSettings = gameSettings;
            this.dataLoader = dataLoader;
            explosionSounds = new List<SoundEffect>();
            standardShellAnimation = new List<Rectangle>();
            standardShellExplosionAnimation = new List<Rectangle>();
            loadingComplete = false;
        }


        /// <inheritdoc/>
        public override void Initialize(ContentWrapper contentWrapper, SpriteBatch spriteBatch)
        {
            base.Initialize(contentWrapper, spriteBatch);
            spriteSetManager = new DataManager<SpritesheetData>(dataLoader);

            standardShellExplosionAnimation = new List<Rectangle>() {
                        new Rectangle(0, 0, 32, 32),
                        new Rectangle(32, 0, 32, 32),
                        new Rectangle(64, 0, 32, 32),
                        new Rectangle(0, 32, 32, 32),
                        new Rectangle(32, 32, 32, 32),
                        new Rectangle(64, 32, 32, 32),
                        new Rectangle(0, 64, 32, 32),
                        new Rectangle(32, 64, 32, 32),
                    };
            standardShellAnimation = new List<Rectangle>() {
                            new Rectangle(0, 0, 32, 32),
                            new Rectangle(32, 0, 32, 32),
                            new Rectangle(64, 0, 32, 32),
                            new Rectangle(0, 32, 32, 32),
                            new Rectangle(32, 32, 32, 32),
                            new Rectangle(64, 32, 32, 32),
                            new Rectangle(0, 64, 32, 32),
                            new Rectangle(32, 64, 32, 32),
                        };
        }

        /// <inheritdoc/>
        public override void LoadContent()
        {
            Func<SpritesheetData, SpriteSheet> conversionFunc = sheet =>
            {
                if (sheet == null)
                {
                    return null;
                }
                Texture2D texture = contentWrapper.Load<Texture2D>(sheet.TextureName);
                return new SpriteSheet(texture, sheet.SingleImageSize.GetPoint(), sheet.DistanceBetweenImages, sheet.Patterns);
            };
            spritesheetToUse = spriteSetManager.GetData(gameSettings.SpriteSetName, conversionFunc);
            healthBarSprite = spriteSetManager.GetData("HealthBarSheet", conversionFunc);
            powerBarSprite = spriteSetManager.GetData("StrengthMeterSheet", conversionFunc);
            defaultShader = contentWrapper.Load<Effect>("Shaders/Default");
            gameFont = contentWrapper.Load<SpriteFont>("gameFont");

            standardShellExplosion = contentWrapper.Load<Texture2D>("Images/Effects/Explosion132x32-Sheet");
            standardShellTexture = contentWrapper.Load<Texture2D>("Images/Entities/BasicMunitionSprite");

            explosionSounds.Add(contentWrapper.Load<SoundEffect>("Sound/Effects/Explosion1"));
            explosionSounds.Add(contentWrapper.Load<SoundEffect>("Sound/Effects/Explosion2"));
            explosionSounds.Add(contentWrapper.Load<SoundEffect>("Sound/Effects/Explosion3"));
            explosionSounds.Add(contentWrapper.Load<SoundEffect>("Sound/Effects/Explosion4"));
        }

        /// <inheritdoc/>
        public override void SetActive()
        {
            base.SetActive();
            Task<MapComponent> mapCreatingTask = mapGenerator.AsyncGenerateNewMap(
                new Point(viewportAdapter.VirtualWidth, viewportAdapter.VirtualHeight),
                new SimpleTexturizer(spritesheetToUse),
                gameSettings.MapSeed
            ); ;
            mapCreatingTask.ContinueWith((component) =>
            {
                //@TODO: Add the map again ... :D
                //map = antecedent.Result;
                CreateEngine();

                uint entity = engine.EntityManager.CreateEntity();
                MapComponent map = component.Result;
                map.RenderRequired = true;
                map.ChangedImageData = component.Result.ImageData;

                VisibleComponent mapRenderer = engine.EntityManager.CreateComponent<VisibleComponent>(entity);
                mapRenderer.SingleTextureSize = new Rectangle(0, 0, component.Result.Width, component.Result.Height);
                mapRenderer.Texture = new Texture2D(TankGame.PublicGraphicsDevice, component.Result.Width, component.Result.Height);
                mapRenderer.Texture.Name = "GeneratedMap";
                mapRenderer.Source = new Rectangle(0, 0, map.Width, map.Height);
                mapRenderer.Destination = new Rectangle(0, 0, map.Width, map.Height);

                PlaceableComponent positionComponent = engine.EntityManager.CreateComponent<PlaceableComponent>(entity);
                positionComponent.Position = Vector2.Zero;
                engine.EntityManager.AddComponent(entity, map);
                SpawnPlayers(component.Result);

                loadingComplete = true;
            });
        }

        /// <summary>
        /// Create the engine ready to use6
        /// </summary>
        private void CreateEngine()
        {
            engine = new GameEngine(new EventManager(), new EntityManager(), contentWrapper);

            Register<IGameObjectBuilder> register = CreateProjectileRegister();
            int screenWidth = viewportAdapter.VirtualWidth;
            int screenHeight = viewportAdapter.VirtualHeight;

            engine.AddSystem(new GameLogicSystem(gameSettings));
            engine.AddSystem(new BindingSystem());
            engine.AddSystem(new KeyboardInputSystem(register));
            engine.AddSystem(new ProjectileSpawnSystem(register));
            engine.AddSystem(new ForceSystem(new VectorRectangle(0, 0, screenWidth, screenHeight)));
            engine.AddSystem(new PhysicSystem(new Rectangle(0, 0, screenWidth, screenHeight), gameSettings.Gravity, gameSettings.Wind));
            engine.AddSystem(new AnimationSystem());
            engine.AddSystem(new DamageSystem());
            engine.AddSystem(new MapSystem());
            engine.AddSystem(new SoundEffectSystem());
            engine.AddSystem(new AnimationAttributeDisplaySystem());
            engine.AddSystem(new FadeInFadeOutSystem());
            engine.AddSystem(new RenderSystem(
                 spriteBatch,
                 gameFont,
                 defaultShader//,
                              //new List<Effect>() { contentWrapper.Load<Effect>("Shaders/Postprocessing/Sepia"), contentWrapper.Load<Effect>("Shaders/Inverted") }
             ));

            MusicManager musicManager = new MusicManager(contentWrapper, new DataManager<Playlist>(new JsonPlaylistLoader()));
            engine.AddSystem(new MusicSystem(musicManager, "IngameMusic"));
        }

        private Register<IGameObjectBuilder> CreateProjectileRegister()
        {
            IRandomizer randomizer = new SystemRandomizer();
            Register<IGameObjectBuilder> register = new Register<IGameObjectBuilder>();
            RegisterProjectile("SimpleExplosive", BaseProjectile(randomizer), register);
            register.SealDictionary();

            return register;
        }

        private void RegisterProjectile(string name, IGameObjectBuilder builder, Register<IGameObjectBuilder> register)
        {
            RegisterProjectile(name, 1, builder, register);
        }

        private void RegisterProjectile(string name, int fireAmount, IGameObjectBuilder builder, Register<IGameObjectBuilder> register)
        {
            register.Add(name, builder);
            ProjectileDataComponent projectileDataComponent = entityManager.CreateComponent<ProjectileDataComponent>();
            projectileDataComponent.Name = name;
            projectileDataComponent.Position = register.GetPosition(name);
            projectileDataComponent.Amount = fireAmount;
            projectileDataComponent.TicksUntilSpawn = 1;

            entityManager.AddComponent(engine.EntityManager.CreateEntity(), projectileDataComponent);
        }

        private IGameObjectBuilder BaseProjectile(IRandomizer randomizer)
        {
            RandomSoundFactory soundFactory = new RandomSoundFactory(explosionSounds, randomizer);
            IGameObjectBuilder explosionBuilder = new BaseExplosionBuilder(standardShellExplosion, standardShellExplosionAnimation, randomizer, soundFactory);
            explosionBuilder.Init(engine);

            BaseBulletBuilder bulletBuilder = new BaseBulletBuilder(standardShellAnimation, standardShellTexture, new ComponentFactory(explosionBuilder));
            bulletBuilder.Init(engine);

            return bulletBuilder;
        }

        /// <summary>
        /// Spawn in all the players
        /// </summary>
        private void SpawnPlayers(MapComponent map)
        {
            int playerSpace = map.Width / (int)(gameSettings.PlayerCount + 1);
            for (int i = 0; i < gameSettings.PlayerCount; i++)
            {
                Player currentPlayer = gameSettings.Players[i];
                int offset = i + 1;
                Vector2 playerStartPosition = new Vector2(playerSpace * offset, 0);
                Raycast raycast = new Raycast(playerStartPosition, Vector2.UnitY, map.Height - 1);
                Point[] positions = raycast.GetPoints();
                Point firstBockedYPos = Array.Find(positions, position =>
                                        {
                                            if (!map.ImageData.IsInArray(position))
                                            {
                                                return false;
                                            }

                                            Color color = map.ImageData.GetValue(position);
                                            return !map.NotSolidColors.Contains(color);
                                        });
                playerStartPosition += Vector2.UnitY * firstBockedYPos.Y;
                currentPlayer.TankBuilder.Init(engine.EntityManager);

                uint playerTank = engine.EntityManager.CreateEntity();

                switch (currentPlayer.ControlType)
                {
                    case Enums.ControlTypeEnum.Keyboard:
                        entityManager.CreateComponent<KeyboardControllerComponent>(playerTank);
                        break;
                    case Enums.ControlTypeEnum.Controller:
                        //engine.EntityManager.CreateComponent<KeyboardControllerComponent>(playerTank);
                        break;
                    default:
                        break;
                }

                currentPlayer.TankBuilder.BuildGameComponents(playerStartPosition).ForEach(component => entityManager.AddComponent(playerTank, component, true));
                ControllableGameObject controllableGameObject = entityManager.GetComponent<ControllableGameObject>(playerTank);
                if (controllableGameObject != null)
                {
                    controllableGameObject.Team = currentPlayer.Team;
                }

                AddEntityEvent addTankEvent = engine.EventManager.CreateEvent<AddEntityEvent>();

                AddPlayerBoundText(-60, currentPlayer.PlayerName, playerTank);
                AddPlayerBoundText(-35, "Team: " + (currentPlayer.Team + 1), playerTank);
                AddEntityEvent addHealthBarBackgroundEvent = engine.EventManager.CreateEvent<AddEntityEvent>();
                addHealthBarBackgroundEvent.Components = AddPlayerHealthBarBackground(20, playerTank);
                engine.EventManager.FireEvent(this, addHealthBarBackgroundEvent);

                AddEntityEvent addHealthBarForegroundEvent = engine.EventManager.CreateEvent<AddEntityEvent>();
                addHealthBarForegroundEvent.Components = AddPlayerHealthBarForeground(20, playerTank);
                engine.EventManager.FireEvent(this, addHealthBarForegroundEvent);

                AddEntityEvent addPowerBarBackgroundEvent = engine.EventManager.CreateEvent<AddEntityEvent>();
                addPowerBarBackgroundEvent.Components = AddPowerBarBackgroundSprite(playerTank);
                engine.EventManager.FireEvent(this, addPowerBarBackgroundEvent);

                AddEntityEvent addPowerBarForegroundEvent = engine.EventManager.CreateEvent<AddEntityEvent>();
                addPowerBarForegroundEvent.Components = AddPowerBarForegroundSprite(playerTank);
                engine.EventManager.FireEvent(this, addPowerBarForegroundEvent);

                uint statistic = entityManager.CreateEntity();
                PlayerStatisticComponent playerStatisticComponent = entityManager.CreateComponent<PlayerStatisticComponent>(statistic);
                playerStatisticComponent.Name = currentPlayer.PlayerName;
                playerStatisticComponent.Team = currentPlayer.Team;

                BindComponent bindComponent = entityManager.CreateComponent<BindComponent>();
                bindComponent.BoundEntityId = playerTank;
                entityManager.AddComponent(statistic, bindComponent, true);
            }
        }

        private uint AddPlayerBoundText(int yOffset, string text, uint targetEntity)
        {
            return AddPlayerBoundText(yOffset, text, targetEntity, Color.White);
        }

        private uint AddPlayerBoundText(int yOffset, string text, uint targetEntity, Color color)
        {
            uint newText = entityManager.CreateEntity();
            entityManager.CreateComponent<PlaceableComponent>(newText);
            VisibleTextComponent textComponent = entityManager.CreateComponent<VisibleTextComponent>(newText);
            textComponent.ShaderEffect = defaultShader;
            textComponent.Text = text;
            textComponent.Color = color;
            textComponent.Font = gameFont;

            BindComponent bindingComponent = entityManager.CreateComponent<BindComponent>();
            bindingComponent.PositionBound = true;
            bindingComponent.Offset = Vector2.UnitY * yOffset;
            bindingComponent.Offset -= Vector2.UnitX * (gameFont.MeasureString(text) / 2);
            bindingComponent.Source = true;
            bindingComponent.BoundEntityId = targetEntity;
            bindingComponent.DeleteIfParentGone = true;

            entityManager.AddComponent(newText, bindingComponent, true);
            return newText;
        }

        private List<IComponent> AddPlayerHealthBarBackground(int yOffset, uint targetEntity)
        {
            List<IComponent> components = new List<IComponent>();
            PlaceableComponent placeableComponent = entityManager.CreateComponent<PlaceableComponent>();
            placeableComponent.Position = Vector2.Zero;

            VisibleComponent visibleComponent = entityManager.CreateComponent<VisibleComponent>();
            visibleComponent.Texture = healthBarSprite.CompleteImage;
            Rectangle backgroundFrame1 = healthBarSprite.GetAreaFromPattern("bFrame1");
            visibleComponent.Destination = backgroundFrame1;
            visibleComponent.Source = backgroundFrame1;
            visibleComponent.SingleTextureSize = backgroundFrame1;
            visibleComponent.LayerDepth = 0;


            AnimationComponent animationComponent = entityManager.CreateComponent<AnimationComponent>();
            animationComponent.FrameSeconds = .25f;
            animationComponent.Loop = true;
            animationComponent.SpriteSources = new List<Rectangle>()
            {
                backgroundFrame1,
                healthBarSprite.GetAreaFromPattern("bFrame2")
            };

            BindComponent bindingComponent = entityManager.CreateComponent<BindComponent>();
            bindingComponent.PositionBound = true;
            bindingComponent.Offset = Vector2.UnitY * yOffset;
            bindingComponent.Offset -= Vector2.UnitX * backgroundFrame1.Width / 2;
            bindingComponent.Source = true;
            bindingComponent.BoundEntityId = targetEntity;
            bindingComponent.DeleteIfParentGone = true;

            components.Add(placeableComponent);
            components.Add(visibleComponent);
            components.Add(animationComponent);
            components.Add(bindingComponent);
            return components;
        }

        private List<IComponent> AddPowerBarBackgroundSprite(uint targetEntity)
        {
            Rectangle backgroundFrame1 = powerBarSprite.GetAreaFromPattern("background");

            List<IComponent> components = new List<IComponent>();
            PlaceableComponent placeableComponent = entityManager.CreateComponent<PlaceableComponent>();
            Vector2 position = Vector2.UnitY * (viewportAdapter.VirtualHeight);
            position += Vector2.UnitX * (viewportAdapter.VirtualWidth - backgroundFrame1.Height);
            placeableComponent.Position = position;
            placeableComponent.Rotation = MathHelper.ToRadians(270);

            VisibleComponent visibleComponent = entityManager.CreateComponent<VisibleComponent>();
            visibleComponent.Texture = powerBarSprite.CompleteImage;
            visibleComponent.Destination = backgroundFrame1;
            visibleComponent.Source = backgroundFrame1;
            visibleComponent.SingleTextureSize = backgroundFrame1;
            visibleComponent.LayerDepth = 0;
            visibleComponent.Hidden = true;

            AnimationComponent animationComponent = entityManager.CreateComponent<AnimationComponent>();
            animationComponent.FrameSeconds = .25f;
            animationComponent.Loop = true;
            animationComponent.SpriteSources = new List<Rectangle>()
            {
                backgroundFrame1,
            };

            BindComponent bindingComponent = entityManager.CreateComponent<BindComponent>();
            bindingComponent.Source = true;
            bindingComponent.BoundEntityId = targetEntity;
            bindingComponent.DeleteIfParentGone = true;

            components.Add(placeableComponent);
            components.Add(visibleComponent);
            components.Add(animationComponent);
            components.Add(bindingComponent);
            components.Add(entityManager.CreateComponent<RoundDependingTag>());
            return components;
        }

        private List<IComponent> AddPlayerHealthBarForeground(int yOffset, uint targetEntity)
        {
            List<IComponent> components = new List<IComponent>();
            PlaceableComponent placeableComponent = entityManager.CreateComponent<PlaceableComponent>();
            placeableComponent.Position = Vector2.Zero;

            VisibleComponent visibleComponent = entityManager.CreateComponent<VisibleComponent>();
            visibleComponent.Texture = healthBarSprite.CompleteImage;
            Rectangle backgroundFrame1 = healthBarSprite.GetAreaFromPattern("fFrame1");
            visibleComponent.Destination = backgroundFrame1;
            visibleComponent.Source = backgroundFrame1;
            visibleComponent.SingleTextureSize = backgroundFrame1;
            visibleComponent.LayerDepth = 1;

            AnimationComponent animationComponent = entityManager.CreateComponent<AnimationComponent>();
            animationComponent.FrameSeconds = .25f;
            animationComponent.Loop = true;
            animationComponent.SpriteSources = new List<Rectangle>()
            {
                backgroundFrame1,
                healthBarSprite.GetAreaFromPattern("fFrame2")
            };

            BindComponent bindingComponent = entityManager.CreateComponent<BindComponent>();
            bindingComponent.PositionBound = true;
            bindingComponent.Offset = Vector2.UnitY * yOffset;
            bindingComponent.Offset -= Vector2.UnitX * backgroundFrame1.Width / 2;
            bindingComponent.Source = true;
            bindingComponent.BoundEntityId = targetEntity;
            bindingComponent.DeleteIfParentGone = true;

            AttributeDisplayComponent attributeDisplayComponent = entityManager.CreateComponent<AttributeDisplayComponent>();
            attributeDisplayComponent.AttributeToDisplay = "Health";
            attributeDisplayComponent.MaxAttributeName = "MaxHealth";

            components.Add(placeableComponent);
            components.Add(visibleComponent);
            components.Add(animationComponent);
            components.Add(bindingComponent);
            components.Add(attributeDisplayComponent);
            return components;
        }
        private List<IComponent> AddPowerBarForegroundSprite(uint targetEntity)
        {
            Rectangle backgroundFrame1 = powerBarSprite.GetAreaFromPattern("foreground");

            List<IComponent> components = new List<IComponent>();
            PlaceableComponent placeableComponent = entityManager.CreateComponent<PlaceableComponent>();
            Vector2 position = Vector2.UnitY * (viewportAdapter.VirtualHeight);
            position += Vector2.UnitX * (viewportAdapter.VirtualWidth - backgroundFrame1.Height);
            placeableComponent.Position = position;
            placeableComponent.Rotation = MathHelper.ToRadians(270);

            VisibleComponent visibleComponent = entityManager.CreateComponent<VisibleComponent>();
            visibleComponent.Texture = powerBarSprite.CompleteImage;
            visibleComponent.Destination = backgroundFrame1;
            visibleComponent.Source = backgroundFrame1;
            visibleComponent.SingleTextureSize = backgroundFrame1;
            visibleComponent.LayerDepth = 1;
            visibleComponent.Hidden = true;

            AnimationComponent animationComponent = entityManager.CreateComponent<AnimationComponent>();
            animationComponent.FrameSeconds = .25f;
            animationComponent.Loop = true;
            animationComponent.SpriteSources = new List<Rectangle>()
            {
                backgroundFrame1,
            };

            AttributeDisplayComponent attributeDisplayComponent = entityManager.CreateComponent<AttributeDisplayComponent>();
            attributeDisplayComponent.AttributeToDisplay = "Strength";
            attributeDisplayComponent.MaxAttributeName = "MaxStrength";

            BindComponent bindingComponent = entityManager.CreateComponent<BindComponent>();
            bindingComponent.Source = true;
            bindingComponent.BoundEntityId = targetEntity;
            bindingComponent.DeleteIfParentGone = true;

            components.Add(placeableComponent);
            components.Add(visibleComponent);
            components.Add(animationComponent);
            components.Add(bindingComponent);
            components.Add(attributeDisplayComponent);
            components.Add(entityManager.CreateComponent<RoundDependingTag>());
            return components;
        }

        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {
            engine?.Update(gameTime);
            if (loadingComplete && entityManager.GetEntitiesWithComponent<MapComponent>().Select(id => entityManager.GetComponent<MapComponent>(id)).All(component => !component.RenderRequired))
            {
                IState gameState = new GameState(engine, gameSettings);
                gameStateManager.Replace(gameState);
            }
        }
    }
}
