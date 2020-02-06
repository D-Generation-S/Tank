using Microsoft.Xna.Framework;
using Tank.Interfaces.Implementations;

namespace Tank.Interfaces.Entity
{
    interface IEntity : IInitializableEntity
    {
        string UniqueName { get; }

        bool Active { get; }

        bool Alive { get; }

        void Initzialize(string uniqueName);

        void Update(GameTime gameTime);

        //void Reset();
    }
}
