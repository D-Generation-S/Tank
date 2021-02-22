using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Tank.Interfaces;

namespace Tank.Code
{
    public class Renderer
    {

        private static Renderer instance;

        public static Renderer Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Renderer();
                }
                return instance;
            }
        }
        public List<IRenderObj> objects;

        public List<IRenderObj> overlayerObjects;

        public void RemoveAllRenderPixels()
        {
            for (int i = objects.Count - 1; i >= 0; i--)
            {
                if (objects[i].GetType() == typeof(DynamicPixel))
                {
                    Remove(objects[i]);
                }
            }
        }


        private Renderer()
        {
            objects = new List<IRenderObj>();
            overlayerObjects = new List<IRenderObj>();
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (IRenderObj obj in objects)
                obj.Draw(sb);
        }

        public void DrawOverlayer(SpriteBatch sb)
        {
            foreach (IRenderObj obj in overlayerObjects)
                obj.Draw(sb);
        }

        public void Add(IRenderObj obj)
        {
            objects.Add(obj);
        }

        public void EndRound()
        {
            objects.Clear();
        }
        public void Remove(IRenderObj obj)
        {
            objects.Remove(obj);
        }

        public void OverlayerAdd(IRenderObj obj)
        {
            overlayerObjects.Add(obj);
        }
        public void OverlayerRemove(IRenderObj obj)
        {
            overlayerObjects.Remove(obj);
        }
        public void OverlayerClear()
        {
            overlayerObjects.Clear();
        }
    }
}
