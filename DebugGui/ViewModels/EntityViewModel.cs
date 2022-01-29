using DebugFramework.DataTypes.SubTypes;
using System.Collections.Generic;

namespace DebugGui.ViewModels
{
    public class EntityViewModel : ViewModelBase
    {
        public uint EntityId { get; set; }
        public int ComponentCount => ComponentView.ComponentCount;

        public ComponentsViewModel ComponentView;

        public EntityViewModel() : this(0, new List<Component>())
        {

        }

        public EntityViewModel(uint entityId, List<Component> components)
        {
            EntityId = entityId;
            ComponentView = new ComponentsViewModel(components);
        }

        public void UpdateComponents(IEnumerable<Component> components)
        {

        }
    }
}
