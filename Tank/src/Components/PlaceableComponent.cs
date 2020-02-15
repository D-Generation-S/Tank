using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tank.src.Components
{
    class PlaceableComponent : BaseComponent
    {
        private Vector2 position;
        public Vector2 Position
        {
            get => position;
            set => position = value;
        }

        private float rotation;
        public float Rotation
        {
            get => rotation;
            set => rotation = value;
        }

        public PlaceableComponent()
        {
            allowMultiple = false;
        }

    }
}
