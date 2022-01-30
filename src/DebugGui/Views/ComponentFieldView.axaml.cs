using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace DebugGui.Views
{
    public partial class ComponentFieldView : UserControl
    {
        public ComponentFieldView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
