using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace DBRegby.Views
{
    public partial class TeamTableView : UserControl
    {
        public TeamTableView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
