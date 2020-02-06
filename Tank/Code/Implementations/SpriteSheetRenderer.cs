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
        private readonly Position singleImageSize;
        private readonly int additionalDistance;
        protected readonly Position position;

        public SpriteSheetRenderer(Position singleImageSize, int additionalDistance, Position position)
        {
            this.singleImageSize = singleImageSize;
            this.additionalDistance = additionalDistance;
            this.position = position;
        }

        protected override void BuildSourceRectangle()
        {
            int positionX = singleImageSize.X * position.X + additionalDistance * position.X;
            int positionY = singleImageSize.Y * position.Y + additionalDistance * position.Y;

            source = new Rectangle(positionX, positionY, singleImageSize.X, singleImageSize.Y);
        }
    }
}
