namespace Tank.Systems.Data
{
    /// <summary>
    /// Class to define static values for controllers
    /// </summary>
    public static class ControlStaticValues
    {
        /// <summary>
        /// Strenght change per tick
        /// </summary>
        public static float STRENGTH_CHANGE = .02f;

        /// <summary>
        /// Rotation change per tick
        /// </summary>
        public static float BARREL_ROTATION_DEGREE = .7f;

        /// <summary>
        /// Min strenght to use
        /// </summary>
        public static float MIN_STRENGHT = 0.2f;

        /// <summary>
        /// Max strenght to use
        /// </summary>
        public static float MAX_STRENGHT = 15f;

        /// <summary>
        /// Max barrel to the right degree
        /// </summary>
        public static float MAX_BARREL_RIGHT = 360;

        /// <summary>
        /// Max barrel to the left degree
        /// </summary>
        public static float MAX_BARREL_LEFT = 180;
    }
}
