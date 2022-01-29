namespace DebugGui.ViewModels
{
    public class ComponentViewModel : ViewModelBase
    {
        public string ComponentName { get; set; }

        public ComponentViewModel(string componentName)
        {
            ComponentName = componentName;
        }
    }
}
