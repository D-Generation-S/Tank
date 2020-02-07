using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tank.Code.BaseClasses;
using Tank.Code.DataContainer;
using Tank.Interfaces.MapGenerators;

namespace Tank.Code.MapGenerators
{
    class DefaultMap : BaseEntity, IMap
    {
        private Texture2D image;
        public Texture2D Image => image;

        private FlattenArray<bool> collissionMap;
        public FlattenArray<bool> CollisionMap => collissionMap;

        public DefaultMap(Texture2D image, FlattenArray<bool> collistionMap)
        {
            
        }
    }
}
