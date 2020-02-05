using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tank.Interfaces.Implementations
{
    interface IRenderer
    {
        Rectangle DrawingContainer { get; }

        Texture2D Texture { get; }

        void DrawStep();
    }
}
