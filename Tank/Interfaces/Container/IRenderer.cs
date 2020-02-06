using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tank.Interfaces.Components;

namespace Tank.Interfaces.Implementations
{
    interface IRenderer : IPlaceable
    {
        Vector2 Size { get; set; }

        Rectangle Destination { get;  }

        Rectangle Source { get;  }

        bool TextureLocked { get; }

        bool IsReady { get; }

        Texture2D Texture { get; set;  }

        void Reset();

        void DrawStep(GameTime gameTime);
    }
}
