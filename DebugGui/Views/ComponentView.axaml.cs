using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace DebugGui.Views
{
    public partial class ComponentView : UserControl
    {
        public ComponentView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
