using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace DBRegby.Views
{
    public partial class PlayerTableView : UserControl
    {
        public PlayerTableView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
