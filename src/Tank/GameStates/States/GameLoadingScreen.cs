using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tank.Builders;
using Tank.DataManagement;
using Tank.DataManagement.Loader;
using Tank.DataStructure;
using Tank.DataStructure.Geometrics;
using Tank.DataStructure.Settings;
using Tank.DataStructure.Spritesheet;
using Tank.EntityComponentSystem.Manager;
using Tank.Events.EntityBased;
using Tank.GameStates.Data;
using Tank.Interfaces.EntityComponentSystem.Manager;
using Tank.Interfaces.MapGenerators;
using Tank.Map.Textureizer;
using Tank.Music;
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
        /// The map to use
        /// </summary>
        private IMap map;

        /// <summary>
        /// The game engine to use
        /// </summary>
        private IGameEngine engine;

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
            loadingComplete = false;
        }


        /// <inheritdoc/>
        public override void Initialize(ContentWrapper contentWrapper, SpriteBatch spriteBatch, ApplicationSettings applicationSettings)
        {
            base.Initialize(contentWrapper, spriteBatch, applicationSettings);
            spriteSetManager = new DataManager<SpriteSheet>(contentWrapper, dataLoader);
        }

        /// <inheritdoc/>
        public override void LoadContent()
        {
            spritesheetToUse = spriteSetManager.GetData(gameSettings.SpriteSetName);
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

            int screenWidth = viewportAdapter.VirtualWidth;
            int screenHeight = viewportAdapter.VirtualHeight;
            engine.AddSystem(new BindingSystem());
            engine.AddSystem(new MapSculptingSystem());
            engine.AddSystem(new ForceSystem(new VectorRectangle(0, 0, screenWidth, screenHeight)));
            engine.AddSystem(new PhysicSystem(new Rectangle(0, 0, screenWidth, screenHeight), gameSettings.Gravity, gameSettings.Wind));
            engine.AddSystem(new AnimationSystem());
            engine.AddSystem(new DamageSystem());
            engine.AddSystem(new SoundEffectSystem(settings));
            engine.AddSystem(new FadeInFadeOutSystem());
            engine.AddSystem(new RenderSystem(
                 spriteBatch,
                 defaultShader//,
                 //new List<Effect>() { contentWrapper.Load<Effect>("Shaders/Postprocessing/Sepia"), contentWrapper.Load<Effect>("Shaders/Inverted") }
             ));
            engine.AddSystem(new GameLogicSystem(gameSettings));

            MusicManager musicManager = new MusicManager(contentWrapper, new DataManager<Music.Playlist>(contentWrapper, new JsonPlaylistLoader()));
            engine.AddSystem(new MusicSystem(musicManager, "IngameMusic", settings));
        }

        /// <summary>
        /// Spawn in all the players
        /// </summary>
        private void SpawnPlayers()
        {
            int playerSpace = map.Width / (int)(gameSettings.PlayerCount + 1);
            for (int i = 0; i < gameSettings.PlayerCount; i++)
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
                    if (!map.IsPixelSolid(position))
                    {
                        playerStartPosition += Vector2.UnitY * position.Y;
                        break;
                    }
                }

                TankObjectBuilder tankObjectBuilder = new TankObjectBuilder(
                    playerStartPosition,
                    contentWrapper.Load<Texture2D>("Images/Entities/BasicTank"),
                    animationFrames
                 );
                tankObjectBuilder.Init(engine.EntityManager);

                AddEntityEvent addEnttiyEvent = engine.EventManager.CreateEvent<AddEntityEvent>();
                addEnttiyEvent.Components = tankObjectBuilder.BuildGameComponents();
                engine.EventManager.FireEvent(this, addEnttiyEvent);
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
