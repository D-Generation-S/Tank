using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tank.Code.DataContainer;

namespace Tank.Interfaces.MapGenerators
{
    interface IMapGenerator
    {
        IMap GenerateNewMap(Position size, int seed);

        IMap AsyncGenerateNewMap(Position size, int seed);
    }
}
