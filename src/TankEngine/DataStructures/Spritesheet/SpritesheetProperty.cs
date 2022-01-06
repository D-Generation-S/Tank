namespace TankEngine.DataStructures.Spritesheet
{
    public class SpritesheetProperty
    {
        public string Name { get; }
        public string Value { get; }

        public SpritesheetProperty(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}
