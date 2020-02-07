﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tank.Code.BaseClasses;
using Tank.Interfaces.Components;
using Tank.Interfaces.Entity;
using Tank.Interfaces.Implementations;

namespace Tank.Code.Entities
{
    class DrawableEntity : BaseEntity, IVisibleEntity
    {
        private IRenderer renderer;
        public IRenderer Renderer => renderer;

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

        private Vector2 rotationAxis;
        public Vector2 RotationAxis
        {
            get => rotationAxis;
            set => rotationAxis = value;
        }

        public DrawableEntity(IRenderer renderer)
        {
            this.renderer = renderer;
            rotationAxis = new Vector2(100, 100);
        }

        public override void Initzialize(string uniqueName)
        {
            base.Initzialize(uniqueName);
            Vector2 tempRotation = rotationAxis;
            if (renderer.IsReady)
            {
                rotationAxis = new Vector2(renderer.TextureSize.X / 2, renderer.TextureSize.Y / 2);
            }
        }

        public override void Update(GameTime gameTime)
        {
            renderer.Position = position;
        }
    }
}
