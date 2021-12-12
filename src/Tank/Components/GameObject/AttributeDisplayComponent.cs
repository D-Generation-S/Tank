namespace Tank.Components.GameObject
{
    class AttributeDisplayComponent : BaseComponent
    {
        /// <summary>
        /// The attribute to display
        /// </summary>
        public string AttributeToDisplay;

        /// <summary>
        /// The attribute to display
        /// </summary>
        public string MaxAttributeName;

        /// <summary>
        /// If set we should show the max attribute as well
        /// </summary>
        public bool MaxAttributePresent => MaxAttributeName != string.Empty;

        /// <inheritdoc/>
        public override void Init()
        {
            AttributeToDisplay = string.Empty;
            MaxAttributeName = string.Empty;
        }
    }
}
