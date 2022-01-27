using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace DebugGui.Views
{
    public partial class GameDebugInstanceView : UserControl
    {
        public GameDebugInstanceView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
