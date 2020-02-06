using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tank.Code.DataContainer;

namespace Tank.Code.Implementations
{
    class SpriteSheetRenderer : SpriteRenderer
    {
        protected readonly Position singleImageSize;
        protected readonly int additionalDistance;
        protected Position sheetPosition;

        public SpriteSheetRenderer(Position singleImageSize, int additionalDistance, Position sheetStartPosition)
        {
            this.singleImageSize = singleImageSize;
            this.additionalDistance = additionalDistance;
            sheetPosition = sheetStartPosition;
        }

        protected override void BuildSourceRectangle()
        {
            int positionX = singleImageSize.X * sheetPosition.X + additionalDistance * sheetPosition.X;
            int positionY = singleImageSize.Y * sheetPosition.Y + additionalDistance * sheetPosition.Y;

            source = new Rectangle(positionX, positionY, singleImageSize.X, singleImageSize.Y);
        }
    }
}
