using System;
using System.Collections.Generic;
using System.Text;

namespace Tank.Components.GameObject
{
    class ProjectileSpawnComponent : BaseComponent
    {
        public int ProjectileToSpawn;

        public float Strenght;

        public float Offset;

        public float Amount;

        public bool UseParentEntity;
        public uint ParentEntity;

        public int TicksUntilSpawn;
        public int CurrentTick;

        public override void Init()
        {
            ProjectileToSpawn = -1;
            Strenght = 0;
            Offset = 0;

            Amount = 0;
            UseParentEntity = false;
            ParentEntity = 0;

            TicksUntilSpawn = 1;
            CurrentTick = 0;
        }
    }
}
