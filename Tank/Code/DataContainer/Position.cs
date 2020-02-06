using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tank.Code.DataContainer
{
    public class Position
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Position()
        {

        }

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
