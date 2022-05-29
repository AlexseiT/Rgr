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
    public partial class DataBaseView : UserControl
    {
        public DataBaseView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void SelectedTabcontrol(object control, SelectionChangedEventArgs args)
        {
            TabControl? tabControl = control as TabControl;
            if (tabControl != null)
            {
                DataBaseViewModel? context = this.DataContext as DataBaseViewModel;
                Table? table = tabControl.SelectedItem as Table;
                if (context != null && table != null)
                {
                    context.CurrentTableName = table.Title;
                }
            }
        }
    }
}
