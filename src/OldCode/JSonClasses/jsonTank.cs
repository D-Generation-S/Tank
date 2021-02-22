using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Tank.Code.Entities;
using Microsoft.Xna.Framework;
using Tank.Code.SubClasses;
using Tank.src.DataStructure;

namespace Tank.Code.JSonClasses
{
    public class jsonTank
    {
        public int ID
        {
            get; set;
        }
        public string Name
        {
            get; set;
        }
        public string Description
        {
            get; set;
        }
        public int Armor
        {
            get; set;
        }
        public string BaseTexture
        {
            get; set;
        }
        public string CanonTexture
        {
            get; set;
        }
        public int minPower
        {
            get; set;
        }
        public int maxPower
        {
            get; set;
        }
        public int Width
        {
            get; set;
        }
        public int Height
        {
            get; set;
        }
        public List<int> Projectiles
        {
            get; set;
        }
        public Position CanonPosition
        {
            get; set;
        }

        public Vehicle CreateVehicle()
        {
            return new Vehicle()
            {
                Armor = Armor,
                Body = TankGame.PublicContentManager.Load<Texture2D>(BaseTexture),
                Canon = TankGame.PublicContentManager.Load<Texture2D>(CanonTexture),
                Width = Width,
                Height = Height,
                MinPower = minPower,
                MaxPower = maxPower,
                CurrentPower = minPower,
            };
        }
    }
}
