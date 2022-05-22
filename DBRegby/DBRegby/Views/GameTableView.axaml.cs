using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace DBRegby.Views
{
    public partial class GameTableView : UserControl
    {
        public GameTableView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
