using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;
using DBRegby.ViewModels;
using DBRegby.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;
using DBRegby.Views;

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
        private void Delete(object control, DataGridAutoGeneratingColumnEventArgs args)
        {
            if (args.PropertyName == "Item" || args.PropertyName == "CompetitionTeams" || args.PropertyName == "GamesNavigation")
            {
                args.Cancel = true;
            }
        }
        private void SelectDeleteRows(object control, SelectionChangedEventArgs args)
        {
            DataGrid? Datagrid = control as DataGrid;
            ViewModelBase? context = this.DataContext as ViewModelBase;
            if (Datagrid != null && context != null)
            {
                if (Datagrid.SelectedItems.Count > 1)
                    return;
                context.Item = Datagrid.SelectedItems;
            }
        }


    }
}
