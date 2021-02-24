using Tank.Interfaces.MapGenerators;

namespace Tank.Components
{
    /// <summary>
    /// This class will make an entity to an map component
    /// </summary>
    class MapComponent : BaseComponent
    {
        /// <summary>
        /// Public readonly access to the map instance
        /// </summary>
        public IMap Map { get; set; }

        public override void Init()
        {
            Map = null;
        }
    }
}
