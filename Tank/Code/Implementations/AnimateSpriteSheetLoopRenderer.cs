using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tank.Code.DataContainer;

namespace Tank.Code.Implementations
{
    class AnimateSpriteSheetLoopRenderer : AnimatedSpriteSheetRenderer
    {
        private bool forward;

        public AnimateSpriteSheetLoopRenderer(Position singleImageSize, int additionalDistance, Position position)
            : this(singleImageSize, additionalDistance, position, 0.5f)
        {
        }

        public AnimateSpriteSheetLoopRenderer(Position singleImageSize, int additionalDistance, Position position, float frameTime)
            : base(singleImageSize, additionalDistance, position, frameTime)
        {
            forward = true;
        }

        protected override void ChangeSprite()
        {
            if (forward)
            {
                base.ChangeSprite();
                if (sheetPosition.X == 0 && sheetPosition.Y == 0)
                {
                    forward = false;
                    sheetPosition.X = sheetDimensions.X;
                    sheetDimensions.Y = sheetDimensions.Y;
                }
                return;
            }

            sheetPosition.X--;
            if (sheetPosition.X <= 0)
            {
                sheetPosition.X = sheetDimensions.X;
                sheetPosition.Y--;
                if (sheetPosition.Y <= 0)
                {
                    sheetPosition.Y = 0;
                    sheetPosition.X = 0;
                    forward = true;
                }
            }
        }
    }
}
