using DebugFramework.DataTypes.SubTypes;
using System.Collections.Generic;

namespace DebugGui.ViewModels
{
    public class EntityViewModel : ViewModelBase
    {
        public uint EntityId { get; set; }
        public int ComponentCount => ComponentView.ComponentCount;

        public ComponentsViewModel ComponentView;

        public EntityViewModel() : this(0, new List<ComponentData>())
        {

        }

        public EntityViewModel(uint entityId, List<ComponentData> components)
        {
            EntityId = entityId;
            ComponentView = new ComponentsViewModel(components);
        }

        public void UpdateComponents(IEnumerable<ComponentData> components)
        {

        }
    }
}
