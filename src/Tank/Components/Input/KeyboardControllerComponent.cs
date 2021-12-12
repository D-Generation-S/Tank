using Microsoft.Xna.Framework.Input;

namespace Tank.Components.Input
{
    /// <summary>
    /// This class represents a keyboard layout
    /// </summary>
    class KeyboardControllerComponent : BaseComponent
    {
        public Keys Debug;
        public Keys RefreshLevel;

        public Keys Menu;
        public Keys Screenshot;
        public Keys Fire;
        public Keys BarrelLeft;
        public Keys BarrelRight;
        public Keys IncreaseStrengh;
        public Keys DecreaseStrengh;
        public Keys NextProjectile;
        public Keys PreviousProjectile;

        public override void Init()
        {
            Debug = Keys.F1;
            RefreshLevel = Keys.F5;

            Menu = Keys.Escape;
            Screenshot = Keys.F12;
            Fire = Keys.Space;
            BarrelLeft = Keys.A;
            BarrelRight = Keys.D;
            IncreaseStrengh = Keys.W;
            DecreaseStrengh = Keys.S;
            NextProjectile = Keys.Right;
            PreviousProjectile = Keys.Left;

        }
    }
}