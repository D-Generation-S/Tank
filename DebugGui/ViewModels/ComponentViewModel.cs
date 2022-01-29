using DebugFramework.DataTypes.SubTypes;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Linq;

namespace DebugGui.ViewModels
{
    public class ComponentViewModel : ViewModelBase
    {
        public bool Selected
        {
            get => selected;
            set => this.RaiseAndSetIfChanged(ref selected, value);
        }

        private bool selected;

        public string ComponentName { get; set; }

        public ObservableCollection<ComponentFieldViewModel> FieldArguments { get; set; }

        public ComponentViewModel(Component component)
        {
            ComponentName = component.ComponentType;
            FieldArguments = new ObservableCollection<ComponentFieldViewModel>(component.Arguments.Select(argument => new ComponentFieldViewModel(argument)));
            FieldArguments.OrderBy(arguments => arguments.Name);
        }
    }
}
