using DebugFramework.DataTypes.SubTypes;
using ReactiveUI;

namespace DebugGui.ViewModels
{
    public class ComponentFieldViewModel : ViewModelBase
    {
        public string Name
        {
            get => name;
            set => this.RaiseAndSetIfChanged(ref name, value);
        }
        private string name;
        public string FieldValue
        {
            get => fieldValue;
            set => this.RaiseAndSetIfChanged(ref fieldValue, value);
        }
        private string fieldValue;

        public ComponentFieldViewModel(ComponentArgument argument)
        {
            Name = argument.Name;
            FieldValue = argument.Value;
        }
    }
}
