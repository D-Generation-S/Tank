using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tank.Interfaces.MapGenerators;

namespace Tank.src.Components
{
    class MapComponent : BaseComponent
    {
        private IMap map;
        public IMap Map => map;

        public MapComponent(IMap map)
        {
            this.map = map;
        }
    }
}
