using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tank.DataManagement;
using Tank.DataManagement.Loader;
using Tank.DataStructure;
using Tank.DataStructure.Spritesheet;
using Tank.GameStates.Data;
using Tank.Interfaces.MapGenerators;
using Tank.Interfaces.Randomizer;
using Tank.Map.Textureizer;
using Tank.Wrapper;

namespace Tank.GameStates.States
{
    class GameLoadingScreen : BaseAbstractState
    {
        private readonly IMapGenerator mapGenerator;
        private readonly GameSettings gameSettings;
        private DataManager<SpriteSheet> spriteSetManager;

        public GameLoadingScreen(IMapGenerator mapGenerator, GameSettings gameSettings)
        {
            this.mapGenerator = mapGenerator;
            this.gameSettings = gameSettings;
        }

        public override void Initialize(ContentWrapper contentWrapper, SpriteBatch spriteBatch)
        {
            base.Initialize(contentWrapper, spriteBatch);

            spriteSetManager = new DataManager<SpriteSheet>(contentWrapper, new MapTextureMockLoader());
        }

        public override void LoadContent()
        {
            
        }

        public override void SetActive()
        {
            Task<IMap> mapCreatingTask = mapGenerator.AsyncGenerateNewMap(
                new Position(
                    1440,
                    900
                ),
                new DefaultTextureizer(spriteSetManager.GetData(gameSettings.SpriteSetName))
            );
            mapCreatingTask.ContinueWith((antecedent) =>
            {
                IState gameState = new GameState(antecedent.Result, gameSettings);
                gameStateManager.Replace(gameState);
            });
        }
    }
}
