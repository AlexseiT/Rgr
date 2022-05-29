using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Input;
using DBRegby.ViewModels;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Avalonia.Interactivity;
using System.Linq;

namespace DBRegby.Views
{
    public partial class RequestManagerView : UserControl
    {
        public RequestManagerView()
        {
            InitializeComponent();

            and = this.FindControl<Button>("and");
            or = this.FindControl<Button>("or");
            pop = this.FindControl<Button>("pop");

            andGroup = this.FindControl<Button>("andGroup");
            orGroup = this.FindControl<Button>("orGroup");
            popGroup = this.FindControl<Button>("popGroup");

            RequestTitle = this.FindControl<TextBox>("RequestTitle");
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void AddNewRequest(object control, RoutedEventArgs args)
        {
            RequestManagerViewModel? DataContext = this.DataContext as RequestManagerViewModel;
            if (DataContext != null)
            {
                DataContext.AddNewRequest(this.FindControl<TextBox>("RequestTitle").Text);
                this.FindControl<Button>("ButtonRequest").IsEnabled = false;
            }
        }
        private void TableChanged(RequestManagerViewModel context)
        {
            bool tableExist = false;
            foreach (Table table in context.AllTables)
            {
                if (table.Title == RequestTitle.Text)
                {
                    tableExist = true;
                    break;
                }
            }
            if (RequestTitle.Text != "" && RequestTitle.Text != null && !tableExist)
                this.FindControl<Button>("ButtonRequest").IsEnabled = true;
            else
                this.FindControl<Button>("ButtonRequest").IsEnabled = false;
        }
        private void ChangedRequest(object control, KeyEventArgs args)
        {
            TextBox? requestName = control as TextBox;
            if (requestName != null)
            {
                var DataContext = this.DataContext as RequestManagerViewModel;
                bool tableExist = false;
                foreach (var table in DataContext.Tables)
                {
                    if (table.Title == requestName.Text)
                    {
                        tableExist = true;
                        break;
                    }
                }
                if (requestName.Text != "" && !tableExist)
                    this.FindControl<Button>("ButtonRequest").IsEnabled = true;
                else
                    this.FindControl<Button>("ButtonRequest").IsEnabled = false;
            }
        }
        private void TableSelected(object control, SelectionChangedEventArgs args)
        {
            ListBox? tablesList = control as ListBox;
            if (tablesList != null)
            {
                RequestManagerViewModel? DataContext = this.DataContext as RequestManagerViewModel;
                if (DataContext != null)
                {
                    DataContext.SelectedTables = new ObservableCollection<Table>();
                    foreach (Table table in tablesList.SelectedItems)
                    {
                        if (!table.checkTable)
                        {
                            DataContext.SelectedTables.Add(table);
                            DataContext.CheckTableSelect = true;
                        }
                        else
                        {
                            DataContext.ClearAll();
                            foreach (Table reqtable in tablesList.SelectedItems)
                            {
                                if (reqtable.checkTable)
                                {
                                    DataContext.SelectedTables.Add(reqtable);
                                }
                            }
                            DataContext.CheckTableSelect = false;
                            break;
                        }
                    }
                    DataContext.JoinTables();

                    if (DataContext.ResultTable.Count == 0)
                    {
                        this.FindControl<Button>("ButtonRequest").IsEnabled = false;
                    }
                    else
                    {
                        TableChanged(DataContext);
                    }
                }
            }
        }
        private void DeleteRequest(object control, RoutedEventArgs args)
        {
            Button? button = control as Button;
            if (button != null)
            {
                RequestManagerViewModel? DataContext = this.DataContext as RequestManagerViewModel;
                if (DataContext != null)
                {
                    DataContext.AllTables.Remove(button.DataContext as Table);
                    DataContext.Requests.Remove(button.DataContext as Table);
                }
            }
        }
        private void ColumnSelected(object control, SelectionChangedEventArgs args)
        {
            ListBox? tablesList = control as ListBox;
            if (tablesList != null)
            {
                RequestManagerViewModel? DataContext = this.DataContext as RequestManagerViewModel;
                if (DataContext != null)
                {
                    DataContext.SelectedColumns.Clear();
                    DataContext.Filters.Clear();
                    DataContext.GroupFilters.Clear();
                    DataContext.Filters.Add(new Filter("", DataContext.SelectedColumns));
                    DataContext.GroupFilters.Add(new Filter("", DataContext.SelectedColumns));
                    foreach (string column in tablesList.SelectedItems)
                    {
                        DataContext.SelectedColumns.Add(column);
                    }

                    DataContext.Selected();

                    if (DataContext.ResultTable.Count == 0)
                    {
                        this.FindControl<Button>("ButtonRequest").IsEnabled = false;
                    }
                    else
                    {
                        TableChanged(DataContext);
                    }

                    if (DataContext.SelectedColumns.Count != 0)
                    {
                        and.IsEnabled = true;
                        or.IsEnabled = true;
                        pop.IsEnabled = true;
                        andGroup.IsEnabled = true;
                        orGroup.IsEnabled = true;
                        popGroup.IsEnabled = true;
                    }
                    else
                    {
                        and.IsEnabled = false;
                        or.IsEnabled = false;
                        pop.IsEnabled = false;
                        andGroup.IsEnabled = false;
                        orGroup.IsEnabled = false;
                        popGroup.IsEnabled = false;
                    }
                }
            }
        }
        private void GroupColumnSelected(object control, SelectionChangedEventArgs args)
        {
            ListBox? columnList = control as ListBox;
            if (columnList != null)
            {
                RequestManagerViewModel? DataContext = this.DataContext as RequestManagerViewModel;
                if (DataContext != null)
                {
                    DataContext.GroupingColumn = columnList.SelectedItem as string;
                }
            }
        }
        private void Back(object control, RoutedEventArgs args)
        {
            RequestManagerViewModel? DataContext = this.DataContext as RequestManagerViewModel;
            MainWindowViewModel? parentContext = this.Parent.DataContext as MainWindowViewModel;
            if (DataContext != null && parentContext != null)
            {
                DataContext.ClearAll();
                parentContext.OpenDB();
            }
        }
        public void AddOr(object control, RoutedEventArgs args)
        {
            RequestManagerViewModel? DataContext = this.DataContext as RequestManagerViewModel;
            Button? button = control as Button;
            if (DataContext != null && button != null)
            {
                string? type = button.CommandParameter as string;
                if (type == "Default")
                {
                    DataContext.Filters.Add(new Filter("OR", DataContext.SelectedColumns));
                    and.IsEnabled = false;
                    pop.IsEnabled = true;
                }
                else
                {
                    DataContext.GroupFilters.Add(new Filter("OR", DataContext.SelectedColumns));
                    andGroup.IsEnabled = false;
                    popGroup.IsEnabled = true;
                }
            }
        }
        public void AddAnd(object control, RoutedEventArgs args)
        {
            RequestManagerViewModel? DataContext = this.DataContext as RequestManagerViewModel;
            Button? button = control as Button;
            if (DataContext != null && button != null)
            {
                string? type = button.CommandParameter as string;
                if (type == "Default")
                {
                    DataContext.Filters.Add(new Filter("AND", DataContext.SelectedColumns));
                    or.IsEnabled = false;
                    pop.IsEnabled = true;
                }
                else
                {
                    DataContext.GroupFilters.Add(new Filter("AND", DataContext.SelectedColumns));
                    orGroup.IsEnabled = false;
                    popGroup.IsEnabled = true;
                }
            }
        }
        public void Popback(object control, RoutedEventArgs args)
        {
            RequestManagerViewModel? DataContext = this.DataContext as RequestManagerViewModel;
            Button? button = control as Button;
            if (DataContext != null && button != null)
            {
                string? type = button.CommandParameter as string;
                if (DataContext.Filters.Count > 1 && type == "Default")
                    DataContext.Filters.Remove(DataContext.Filters.Last());
                else if (DataContext.GroupFilters.Count > 1 && type == "Group")
                    DataContext.GroupFilters.Remove(DataContext.GroupFilters.Last());

                if (DataContext.Filters.Count == 1 && type == "Default")
                {
                     or.IsEnabled = true;
                     and.IsEnabled = true;
                     pop.IsEnabled = false;
                }
                else if (DataContext.GroupFilters.Count == 1 && type == "Group")
                {
                    orGroup.IsEnabled = true;
                    andGroup.IsEnabled = true;
                    popGroup.IsEnabled = false;
                }
            }
        }
        private void ComboBoxChanged(object control, SelectionChangedEventArgs args)
        {
            ComboBox? ComboBox = control as ComboBox;
            if (ComboBox != null)
            {
                Filter? filterContext = ComboBox.DataContext as Filter;
                if (filterContext != null && ComboBox.Name != null)
                {
                    if (ComboBox.Name.Contains("Columns"))
                    {
                        filterContext.Column = ComboBox.SelectedItem as string;
                    }
                    else if (ComboBox.Name.Contains("Operators"))
                    {
                        filterContext.Operator = ComboBox.SelectedItem as string;
                    }
                }
            }
        }
        private void FilterChanged(object control, KeyEventArgs args)
        {
            TextBox? filterValue = control as TextBox;
            if (filterValue != null)
            {
                RequestManagerViewModel? DataContext = this.DataContext as RequestManagerViewModel;
                if (DataContext != null)
                {
                    if (DataContext.ResultTable.Count == 0)
                    {
                        this.FindControl<Button>("ButtonRequest").IsEnabled = false;
                    }
                    else
                    {
                        TableChanged(DataContext);
                    }
                }
            }
        }
    }
}
