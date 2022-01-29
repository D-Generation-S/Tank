using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace DebugGui.Views
{
    public partial class EntityView : UserControl
    {
        public EntityView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
