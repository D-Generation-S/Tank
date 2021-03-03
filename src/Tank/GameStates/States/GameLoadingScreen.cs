using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Threading.Tasks;
using Tank.DataManagement;
using Tank.DataManagement.Loader;
using Tank.DataStructure;
using Tank.DataStructure.Spritesheet;
using Tank.GameStates.Data;
using Tank.Interfaces.MapGenerators;
using Tank.Map.Textureizer;
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
        private readonly IDataLoader<SpriteSheet> dataLoader;

        /// <summary>
        /// The manager used to load the sprites
        /// </summary>
        private DataManager<SpriteSheet> spriteSetManager;

        /// <summary>
        /// The spritesheet to use for the map
        /// </summary>
        private SpriteSheet spritesheetToUse;

        private bool loadingComplete;
        private IMap map;

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
        public override void Initialize(ContentWrapper contentWrapper, SpriteBatch spriteBatch)
        {
            base.Initialize(contentWrapper, spriteBatch);
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
            Task<IMap> mapCreatingTask = mapGenerator.AsyncGenerateNewMap(
                new Position(
                    1440,
                    900
                ),
                new DefaultTextureizer(spritesheetToUse),
                gameSettings.MapSeed
            );
            mapCreatingTask.ContinueWith((antecedent) =>
            {
                map = antecedent.Result;
                loadingComplete = true;
            });
        }

        public override void Update(GameTime gameTime)
        {
            if (loadingComplete)
            {
                IState gameState = new GameState(map, gameSettings);
                gameStateManager.Replace(gameState);
            }
        }
    }
}
