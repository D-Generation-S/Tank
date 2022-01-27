namespace DebugFramework.DataTypes.SubTypes
{
    public class ComponentArgument : BaseDataType
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public ComponentArgument() : this(string.Empty, string.Empty)
        {
        }

        public ComponentArgument(string name, string value)
        {
            this.Name = name;
            this.Value = value;
        }
    }
}
