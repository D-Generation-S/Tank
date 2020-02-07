using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tank.Code.DataContainer;

namespace Tank.Code.Implementations
{
    class AnimatedSpriteSheetRenderer : SpriteSheetRenderer
    {
        private readonly float animationSeconds;

        private float previousTime;
        private float timeThreshold;

        protected Position sheetDimensions;
        protected FlattenArray<bool> imageAvailable;

        protected Rectangle allowedArea;

        protected Position sheetStartPoint;
        protected Position sheetEndPoint;

        public AnimatedSpriteSheetRenderer(Position singleImageSize, int additionalDistance, Position position)
            : this(singleImageSize, additionalDistance, position, 0.5f)
        {
        }

        public AnimatedSpriteSheetRenderer(Position singleImageSize, int additionalDistance, Position position, float frameTime)
            : base(singleImageSize, additionalDistance, position)
        {
            animationSeconds = frameTime;
        }

        public AnimatedSpriteSheetRenderer(Position singleImageSize, int additionalDistance, Position position, float frameTime, Rectangle allowedArea)
            : base(singleImageSize, additionalDistance, position)
        {
            animationSeconds = frameTime;
            this.allowedArea = allowedArea;
        }

        public override void DrawStep(GameTime gameTime)
        {
            float currentTime = (float)gameTime.ElapsedGameTime.Milliseconds;
            timeThreshold += currentTime / 1000;

            if (timeThreshold > animationSeconds)
            {
                timeThreshold = timeThreshold - animationSeconds;
                int attemps = (sheetDimensions.X) * (sheetDimensions.Y);
                for (int i = 0; i < attemps; i++)
                {
                    ChangeSprite();
                    if (imageAvailable.GetValue(sheetPosition))
                    {
                        break;
                    }
                }
                
                BuildSourceRectangle();
            }
        }

        protected virtual void ChangeSprite()
        {
            sheetPosition.X++;
            if (sheetPosition.X > sheetEndPoint.X) 
            {
                sheetPosition.X = sheetStartPoint.X;
                sheetPosition.Y++;
                if (sheetPosition.Y > sheetEndPoint.Y)
                {
                    sheetPosition.Y = sheetStartPoint.Y;
                }
            }
        }

        public override void SetTexture(Texture2D texture)
        {
            base.SetTexture(texture);

            if (allowedArea.Width == 0 || allowedArea.Height == 0)
            {
                allowedArea = new Rectangle(0, 0, texture.Width, texture.Height);
            }

            double xSize = singleImageSize.X + additionalDistance;
            double ySize = singleImageSize.Y + additionalDistance;
            int dimensionX = (int)Math.Floor((allowedArea.Width - allowedArea.X) / xSize);
            int dimensionY = (int)Math.Floor((allowedArea.Height - allowedArea.Y) / ySize);

            sheetDimensions = new Position(dimensionX, dimensionY);
            sheetStartPoint = new Position(
                allowedArea.X / (singleImageSize.X + additionalDistance),
                allowedArea.Y / (singleImageSize.Y + additionalDistance)
            );
            sheetEndPoint = new Position(
                ((allowedArea.Width - allowedArea.X) / (singleImageSize.X + additionalDistance)) - 1,
                ((allowedArea.Height - allowedArea.Y) / (singleImageSize.Y + additionalDistance)) - 1
            );

            BuildImageData();

        }

        private void BuildImageData()
        {
            int startPointX = sheetStartPoint.X;
            int startPointY = sheetStartPoint.Y;
            int dimensionX = sheetDimensions.X;
            dimensionX += startPointX;
            int dimensionY = sheetDimensions.Y;
            dimensionX += startPointY;
            imageAvailable = new FlattenArray<bool>(dimensionX, dimensionY);
            for (int y = startPointY; y < dimensionY; y++)
            {
                for (int x = startPointX; x < dimensionX; x++)
                {
                    bool dataPreset = ImageContainsData(
                        x * singleImageSize.X + additionalDistance,
                        y * singleImageSize.Y + additionalDistance
                    );
                    imageAvailable.SetValue(x, y, dataPreset);
                }
            }
        }

        private bool ImageContainsData(int xStart, int yStart)
        {
            Color[] data = new Color[texture.Width * texture.Height];
            texture.GetData<Color>(data);

            for (int y = yStart; y < yStart + singleImageSize.Y; y++)
            {
                for (int x = xStart; x < xStart + singleImageSize.X; x++)
                {
                    int target = y * texture.Width + x;
                    Color color = data[target];
                    if (color.R > 0 || color.G > 0 || color.B > 0)
                    {
                        return true;
                    }

                }
            }
            return false;
        }
    }
}
