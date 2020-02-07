using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tank.Code.DataContainer;
using Tank.Interfaces.Entity;

namespace Tank.Interfaces.MapGenerators
{
    interface IMap : IEntity
    {
        /// <summary>
        /// The image to draw the map on the screen
        /// </summary>
        Texture2D Image { get; }

        /// <summary>
        /// The collision data for this map
        /// </summary>
        FlattenArray<bool> CollisionMap { get; }
    }
}
