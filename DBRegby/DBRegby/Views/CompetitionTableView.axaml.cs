using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace DBRegby.Views
{
    public partial class CompetitionTableView : UserControl
    {
        public CompetitionTableView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
