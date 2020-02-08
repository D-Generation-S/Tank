using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tank.Code.DataContainer;

namespace Tank.Code.Implementations
{
    class AnimatedSpriteSheetLoopRenderer : AnimatedSpriteSheetRenderer
    {
        private bool forward;

        public AnimatedSpriteSheetLoopRenderer(Position singleImageSize, int additionalDistance, Position position)
            : this(singleImageSize, additionalDistance, position, 0.5f)
        {
        }

        public AnimatedSpriteSheetLoopRenderer(Position singleImageSize, int additionalDistance, Position position, float frameTime)
            : base(singleImageSize, additionalDistance, position, frameTime)
        {
            forward = true;
        }

        public AnimatedSpriteSheetLoopRenderer(Position singleImageSize, int additionalDistance, Position position, float frameTime, Rectangle allowedArea)
            : base(singleImageSize, additionalDistance, position)
        {
            forward = true;
            this.allowedArea = allowedArea;
        }

        protected override void ChangeSprite()
        {
            if (forward)
            {
                base.ChangeSprite();
                if (sheetPosition.X == sheetStartPoint.X && sheetPosition.Y == sheetStartPoint.Y)
                {
                    forward = false;
                    for (int x = sheetEndPoint.X; x >= 0; x--)
                    {
                        if (imageAvailable.GetValue(x, sheetEndPoint.Y))
                        {
                            sheetPosition.X = x;
                        }
                        
                    }
                    
                    sheetPosition.Y = sheetEndPoint.Y;
                }
            }

            if (!forward)
            {
                sheetPosition.X--;
                if (sheetPosition.X < sheetStartPoint.X)
                {
                    sheetPosition.X = sheetEndPoint.X;
                    sheetPosition.Y--;
                    if (sheetPosition.Y < sheetStartPoint.Y)
                    {
                        sheetPosition.Y = sheetEndPoint.X != 0 ? sheetStartPoint.Y : sheetStartPoint.Y + 1;
                        sheetPosition.X = sheetEndPoint.Y == 0 ? sheetStartPoint.X : sheetStartPoint.X + 1;
                        forward = true;
                    }
                }
            }
        }
    }
}
