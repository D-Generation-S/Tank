using Microsoft.Xna.Framework;
using System;

namespace Tank.Code
{
    class Explode
    {
        public Explode(int xPos, int yPos, float radius)
        {
            float radiusSq = radius * radius;

            if (Settings.ExplosionSound != null)
                Settings.ExplosionSound.Play(Settings.SoundVolume, 0, 0);

            for (int x = xPos - (int)radius; x < xPos + (int)radius; x += Terrain.Instance.destructionRes)
            {
                if (x >= 0 && x < Terrain.Instance.Width)
                {
                    for (int y = yPos - (int)radius; y < yPos + (int)radius; y += Terrain.Instance.destructionRes)
                    {

                        if (y >= 0 && y < Terrain.Instance.Height)
                        {
                            int solidX = 0, solidY = 0;
                            bool solid = false;

                            for (int i = 0; i < Terrain.Instance.destructionRes && !solid; i++)
                            {
                                for (int j = 0; j < Terrain.Instance.destructionRes && !solid; j++)
                                {
                                    if (Terrain.IsPixelSolid(x + i, y + j))
                                    {
                                        solid = true;
                                        solidX = x + i;
                                        solidY = y + j;
                                    }
                                }
                            }
                            if (solid)
                            {
                                float xDiff = x - xPos;
                                float yDiff = y - yPos;
                                float distSq = xDiff * xDiff + yDiff * yDiff;

                                if (distSq < radiusSq)
                                {
                                    float distance = (float)Math.Sqrt(distSq);

                                    float speed = 800 * (1 - distance / radius);

                                    if (distance == 0)
                                        distance = 0.001f;

                                    float velX = MathHelper.Clamp(speed * (xDiff + new Random().Next(-10, 10)) / distance, -1000, 1000);
                                    float velY = MathHelper.Clamp(speed * (yDiff + new Random().Next(-10, 10)) / distance, -1000, 1000);

                                    DynamicPixel pixel = new DynamicPixel(Terrain.GetPixel(solidX, solidY), x, y, velX, velY, Terrain.Instance.destructionRes, true);
                                    pixel.stickiness = 800;
                                    Physics.Instance.Add(pixel);
                                    Renderer.Instance.Add(pixel);

                                    for (int i = 0; i < Terrain.Instance.destructionRes; i++)
                                    {
                                        for (int j = 0; j < Terrain.Instance.destructionRes; j++)
                                        {
                                            Terrain.RemovePixel(x + i, y + j);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
