using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tank.Code.DataContainer;
using Tank.Code.Implementations;
using Tank.Interfaces.Builder;
using Tank.Interfaces.Implementations;

namespace Tank.Code.Builder
{
    class StaticProjectileBuilder : IBuilder<Entities.Weapon.Projectile>
    {
        private readonly Texture2D texture;

        public StaticProjectileBuilder(Texture2D texture)
        {
            this.texture = texture;
        }

        public Entities.Weapon.Projectile Build()
        {
            Position imageSize = new Position(32, 32);
            Position framePosition = new Position(0, 0);
            IRenderer renderer = new AnimateSpriteSheetLoopRenderer(imageSize, 0, framePosition, .05f)
            {
                Position = new Vector2(10, 10)
            };
            renderer.SetTexture(texture);

            return new Entities.Weapon.Projectile(renderer)
            {
                Velocity = new Vector2(0.5f, 0)
            };

        }
    }
}
