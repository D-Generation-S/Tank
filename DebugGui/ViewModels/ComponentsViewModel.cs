using DebugFramework.DataTypes.SubTypes;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace DebugGui.ViewModels
{
    public class ComponentsViewModel : ViewModelBase
    {
        public int ComponentCount => Components.Count;
        public ObservableCollection<ComponentViewModel> Components { get; set; }

        public ComponentsViewModel(List<Component> components)
        {
            Components = new ObservableCollection<ComponentViewModel>(components.Select(component => new ComponentViewModel(component.ComponentType)));
        }

        public void UpdateComponents(List<Component> components)
        {

        }
    }
}
