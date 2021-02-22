using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Tank.Interfaces;

namespace Tank.Code
{
    public class DynamicPixel : IPhysicsObj, IRenderObj
    {
        float LastX;
        float LastY;

        public float stickiness = 150; // minimum speed for this pixel to stick
        float bounceFriction = 0.4f; // scalar multiplied to velocity after bouncing

        Color col;

        int Size = 1;

        int TTL = 512;
        private bool _KillAfterTTL;

        public float velX
        {
            get;
            set;
        }
        public float velY
        {
            get;
            set;
        }

        public float x
        {
            get;
            set;
        }
        public float y
        {
            get;
            set;
        }

        public DynamicPixel(Color c, float X, float Y, float vX, float vY, int size, bool KillAfterTTL = false)
        {
            col = c;
            x = X;
            y = Y;
            LastX = x;
            LastY = y;
            velX = vX;
            velY = vY;
            Size = size;

            _KillAfterTTL = KillAfterTTL;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(Settings.DynamicPixelTexture, new Rectangle((int)x, (int)y, Size, Size), col);
        }

        public void checkConstraints(GameTime CurrentGameTime)
        {
            // Boundary constraints... only remove the pixel if it exits the sides or bottom of the map
            if ((x > Terrain.Instance.Width) || x < 0 || y > Terrain.Instance.Height || y < -Terrain.Instance.Height)
            {
                Renderer.Instance.Remove(this);
                Physics.Instance.Remove(this);
                return;
            }
            // Find if there's a collision between the current and last points
            int[] collision = Helper.RayCast((int)LastX, (int)LastY, (int)x, (int)y);
            if (collision.Length > 0)
                collide(collision[0], collision[1], collision[2], collision[3]);

            // reset last positions
            LastX = x;
            LastY = y;


            if (_KillAfterTTL)
            {
                if (TTL <= 0)
                {
                    Renderer.Instance.Remove(this);
                    Physics.Instance.Remove(this);
                }
                TTL--;
            }

        }

        /* Collide */
        // called whenever checkConstraints() detects a solid pixel in the way
        void collide(int thisX, int thisY, int thatX, int thatY)
        {
            if (velX * velX + velY * velY < stickiness * stickiness)
            {
                for (int i = 0; i < Size; i++)
                {
                    for (int j = 0; j < Size; j++)
                    {
                        Terrain.AddPixel(col, thisX + i, thisY + j);
                    }
                }

                Renderer.Instance.Remove(this);
                Physics.Instance.Remove(this);
            }
            else
            {
                // to do this, we need to reflect the velocity across the edge normal at the collision point
                // this is done using a 2D vector reflection formula ( http://en.wikipedia.org/wiki/Reflection_(mathematics) )

                float[] pixelNormal = Terrain.Instance.GetNormal(thatX, thatY);

                float d = 2 * (velX * pixelNormal[0] + velY * pixelNormal[1]);

                velX -= pixelNormal[0] * d;
                velY -= pixelNormal[1] * d;

                velX *= bounceFriction;
                velY *= bounceFriction;

                x = thisX;
                y = thisY;
            }
        }
    }
}
