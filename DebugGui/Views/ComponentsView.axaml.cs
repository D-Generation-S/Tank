using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace DebugGui.Views
{
    public partial class ComponentsView : UserControl
    {
        public ComponentsView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
