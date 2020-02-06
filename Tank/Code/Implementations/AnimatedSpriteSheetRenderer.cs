using Microsoft.Xna.Framework;
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

        public AnimatedSpriteSheetRenderer(Position singleImageSize, int additionalDistance, Position position)
    : this(singleImageSize, additionalDistance, position, 0.5f)
        {

        }

        public AnimatedSpriteSheetRenderer(Position singleImageSize, int additionalDistance, Position position, float frameTime)
            : base(singleImageSize, additionalDistance, position)
        {
            animationSeconds = frameTime;
        }

        public override void DrawStep(GameTime gameTime)
        {
            float currentTime = (float)gameTime.ElapsedGameTime.Milliseconds;
            timeThreshold += currentTime / 1000;

            if (timeThreshold > animationSeconds)
            {
                timeThreshold = timeThreshold - animationSeconds;
                ChangeSprite();
                BuildSourceRectangle();
            }
        }

        protected virtual void ChangeSprite()
        {
            sheetPosition.X++;
            if (sheetPosition.X > sheetDimensions.X)
            {
                sheetPosition.X = 0;
                sheetPosition.Y++;
                if (sheetPosition.Y > sheetDimensions.Y)
                {
                    sheetPosition.Y = 0;
                }
            }
        }

        protected override void BuildSourceRectangle()
        {
            base.BuildSourceRectangle();

            double xSize = singleImageSize.X + additionalDistance;
            double ySize = singleImageSize.Y + additionalDistance;
            int dimensionX = (int)Math.Floor(texture.Width / xSize) - 1;
            int dimensionY = (int)Math.Floor(texture.Height / ySize) - 1;

            sheetDimensions = new Position(dimensionX, dimensionY);
        }
    }
}
