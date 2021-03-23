using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tank.Builders;
using Tank.Components;
using Tank.Components.DataLookup;
using Tank.Components.GameObject;
using Tank.Components.Input;
using Tank.Components.Rendering;
using Tank.DataManagement;
using Tank.DataManagement.Loader;
using Tank.DataStructure;
using Tank.DataStructure.Geometrics;
using Tank.DataStructure.Settings;
using Tank.DataStructure.Spritesheet;
using Tank.EntityComponentSystem.Manager;
using Tank.Events.EntityBased;
using Tank.Factories;
using Tank.GameStates.Data;
using Tank.Interfaces.Builders;
using Tank.Interfaces.EntityComponentSystem.Manager;
using Tank.Interfaces.MapGenerators;
using Tank.Interfaces.Randomizer;
using Tank.Map.Textureizer;
using Tank.Music;
using Tank.Randomizer;
using Tank.Register;
using Tank.Systems;
using Tank.Utils;
using Tank.Wrapper;

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
        private readonly IDataLoader<SpriteSheet> dataLoader;

        /// <summary>
        /// The manager used to load the sprites
        /// </summary>
        private DataManager<SpriteSheet> spriteSetManager;

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
        /// The map to use
        /// </summary>
        private IMap map;

        /// <summary>
        /// The game engine to use
        /// </summary>
        private IGameEngine engine;

        private List<SoundEffect> explosionSounds;
        private Texture2D standardShellExplosion;
        private Texture2D standardShellTexture;

        private List<Rectangle> standardShellAnimation;
        private List<Rectangle> standardShellExplosionAnimation;

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="mapGenerator">The map generating algorihm to use</param>
        /// <param name="gameSettings">The game settings to use</param>
        public GameLoadingScreen(IMapGenerator mapGenerator, GameSettings gameSettings)
            :this(mapGenerator, gameSettings, new JsonTextureLoader())
        {
        }

        /// <summary>
        /// Create a new instance of this class
        /// </summary>
        /// <param name="mapGenerator">The map generating algorihm to use</param>
        /// <param name="gameSettings">The game settings to use</param>
        /// <param name="dataLoader">The data loader to use</param>
        public GameLoadingScreen(IMapGenerator mapGenerator, GameSettings gameSettings, IDataLoader<SpriteSheet> dataLoader)
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
        public override void Initialize(ContentWrapper contentWrapper, SpriteBatch spriteBatch, ApplicationSettings applicationSettings)
        {
            base.Initialize(contentWrapper, spriteBatch, applicationSettings);
            spriteSetManager = new DataManager<SpriteSheet>(contentWrapper, dataLoader);

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
            spritesheetToUse = spriteSetManager.GetData(gameSettings.SpriteSetName);
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
            Task<IMap> mapCreatingTask = mapGenerator.AsyncGenerateNewMap(
                new Position(
                    viewportAdapter.VirtualWidth,
                    viewportAdapter.VirtualHeight
                ),
                new DefaultTextureizer(spritesheetToUse),
                gameSettings.MapSeed
            );
            mapCreatingTask.ContinueWith((antecedent) =>
            {
                map = antecedent.Result;
                CreateEngine();
                SpawnPlayers();
                loadingComplete = true;
            });
        }

        /// <summary>
        /// Create the engine ready to use
        /// </summary>
        private void CreateEngine()
        {
            engine = new GameEngine(new EventManager(), new EntityManager(), contentWrapper);

            Register<IGameObjectBuilder> register = CreateProjectileRegister();
            int screenWidth = viewportAdapter.VirtualWidth;
            int screenHeight = viewportAdapter.VirtualHeight;
            engine.AddSystem(new BindingSystem());
            engine.AddSystem(new KeyboardInputSystem(register));
            engine.AddSystem(new ProjectileSpawnSystem(register));
            engine.AddSystem(new MapSculptingSystem());
            engine.AddSystem(new ForceSystem(new VectorRectangle(0, 0, screenWidth, screenHeight)));
            engine.AddSystem(new PhysicSystem(new Rectangle(0, 0, screenWidth, screenHeight), gameSettings.Gravity, gameSettings.Wind));
            engine.AddSystem(new AnimationSystem());
            engine.AddSystem(new DamageSystem());
            engine.AddSystem(new SoundEffectSystem(settings));
            engine.AddSystem(new FadeInFadeOutSystem());
            engine.AddSystem(new RenderSystem(
                 spriteBatch,
                 gameFont,
                 defaultShader//,
                 //new List<Effect>() { contentWrapper.Load<Effect>("Shaders/Postprocessing/Sepia"), contentWrapper.Load<Effect>("Shaders/Inverted") }
             ));
            engine.AddSystem(new TextAttributeDisplaySystem());
            engine.AddSystem(new GameLogicSystem(gameSettings));

            MusicManager musicManager = new MusicManager(contentWrapper, new DataManager<Music.Playlist>(contentWrapper, new JsonPlaylistLoader()));
            engine.AddSystem(new MusicSystem(musicManager, "IngameMusic", settings));
        }

        private Register<IGameObjectBuilder> CreateProjectileRegister()
        {
            IRandomizer randomizer = new SystemRandomizer();
            Register<IGameObjectBuilder> register =  new Register<IGameObjectBuilder>();
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
            ProjectileDataComponent projectileDataComponent = engine.EntityManager.CreateComponent<ProjectileDataComponent>();
            projectileDataComponent.Name = name;
            projectileDataComponent.Position = register.GetPosition(name);
            projectileDataComponent.Amount = fireAmount;
            projectileDataComponent.TicksUntilSpawn = 1;

            engine.EntityManager.AddComponent(engine.EntityManager.CreateEntity(), projectileDataComponent);
        }

        private IGameObjectBuilder BaseProjectile(IRandomizer randomizer)
        {
            RandomSoundFactory soundFactory = new RandomSoundFactory(explosionSounds, randomizer);
            IGameObjectBuilder explosionBuilder = new BaseExplosionBuilder(standardShellExplosion, standardShellExplosionAnimation, soundFactory);
            explosionBuilder.Init(engine);

            BaseBulletBuilder bulletBuilder = new BaseBulletBuilder(standardShellAnimation, standardShellTexture, new ComponentFactory(explosionBuilder));
            bulletBuilder.Init(engine);

            return bulletBuilder;

            //RandomEntityBuilderFactory randomExplosionFactory = new RandomEntityBuilderFactory(explosionBuilders, randomizer);
            //bulletBuilder
        }

        /// <summary>
        /// Spawn in all the players
        /// </summary>
        private void SpawnPlayers()
        {
            int playerSpace = map.Width / (int)(gameSettings.PlayerCount + 1);
            for (int i = 0; i < gameSettings.PlayerCount; i++)
            {
                Player currentPlayer = gameSettings.Players[i];
                int offset = i + 1;
                List<Rectangle> animationFrames = new List<Rectangle>();
                animationFrames.Add(new Rectangle(0, 0, 32, 32));
                Vector2 playerStartPosition = new Vector2(playerSpace * offset, 0);
                Raycast raycast = new Raycast(playerStartPosition, Vector2.UnitY, map.Height - 1);
                Position[] positions = raycast.GetPositions();
                for (int pIndex = positions.Length - 1; pIndex > 0; pIndex--)
                {
                    Position position = positions[pIndex];
                    if (!map.IsPixelSolid(position))
                    {
                        playerStartPosition += Vector2.UnitY * position.Y;
                        break;
                    }
                }
                currentPlayer.TankBuilder.Init(engine.EntityManager);

                uint playerTank = engine.EntityManager.CreateEntity();

                switch (currentPlayer.ControlType)
                {
                    case Enums.ControlTypeEnum.Keyboard:
                        engine.EntityManager.CreateComponent<KeyboardControllerComponent>(playerTank);
                        break;
                    case Enums.ControlTypeEnum.Controller:
                        //engine.EntityManager.CreateComponent<KeyboardControllerComponent>(playerTank);
                        break;
                    default:
                        break;
                }

                currentPlayer.TankBuilder.BuildGameComponents(playerStartPosition).ForEach(component => engine.EntityManager.AddComponent(playerTank, component, true));
                ControllableGameObject controllableGameObject = engine.EntityManager.GetComponent<ControllableGameObject>(playerTank);
                if (controllableGameObject != null)
                {
                    controllableGameObject.Team = currentPlayer.Team;
                }

                AddEntityEvent addTankEvent = engine.EventManager.CreateEvent<AddEntityEvent>();

                uint playerName = engine.EntityManager.CreateEntity();
                PlaceableComponent placeableComponent = engine.EntityManager.CreateComponent<PlaceableComponent>(playerName);
                VisibleTextComponent textComponent = engine.EntityManager.CreateComponent<VisibleTextComponent>(playerName);
                textComponent.ShaderEffect = defaultShader;
                textComponent.Text = currentPlayer.PlayerName;
                textComponent.Font = gameFont;

                BindComponent bindComponent = engine.EntityManager.CreateComponent<BindComponent>();
                bindComponent.PositionBound = true;
                bindComponent.Offset = Vector2.UnitY * -35;
                bindComponent.Offset -= Vector2.UnitX * (gameFont.MeasureString(currentPlayer.PlayerName) / 2);
                bindComponent.Source = true;
                bindComponent.BoundEntityId = playerTank;
                bindComponent.DeleteIfParentGone = true;


                engine.EntityManager.AddComponent(playerName, bindComponent, true);

                uint playerLife = engine.EntityManager.CreateEntity();
                PlaceableComponent playerLifePlacement = engine.EntityManager.CreateComponent<PlaceableComponent>(playerLife);
                AttributeDisplayComponent attributeDisplayComponent = engine.EntityManager.CreateComponent<AttributeDisplayComponent>(playerLife);
                attributeDisplayComponent.AttributeToDisplay = "Health";
                attributeDisplayComponent.MaxAttributeName = "MaxHealth";
                VisibleTextComponent playerLifeText = engine.EntityManager.CreateComponent<VisibleTextComponent>(playerLife);
                playerLifeText.ShaderEffect = defaultShader;
                playerLifeText.Text = string.Empty;
                playerLifeText.Font = gameFont;
                playerLifeText.Color = Color.Black;
                BindComponent playerLifeBin = engine.EntityManager.CreateComponent<BindComponent>();
                playerLifeBin.PositionBound = true;
                playerLifeBin.Offset = Vector2.UnitY * 20;
                playerLifeBin.Source = true;
                playerLifeBin.BoundEntityId = playerTank;
                playerLifeBin.DeleteIfParentGone = true;

                engine.EntityManager.AddComponent(playerLife, playerLifeBin, true);
            }
        }

        /// <inheritdoc/>
        public override void Update(GameTime gameTime)
        {
            if (loadingComplete)
            {
                IState gameState = new GameState(map, engine, gameSettings);
                gameStateManager.Replace(gameState);
            }
        }
    }
}
