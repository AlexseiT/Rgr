using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace DBRegby.Views
{
    public partial class CompetitionTeamTableView : UserControl
    {
        public CompetitionTeamTableView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
