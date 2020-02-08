using Tank.Interfaces.Components;

namespace Tank.Interfaces.Entity
{
    interface IPhysicEntity : IEntity, IMoveable
    {
        bool OnGround { get; set; }
    }
}
