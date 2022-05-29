using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using ReactiveUI;
using System.Reactive;
using DBRegby.Models;
using Microsoft.Data.Sqlite;
using System.IO;
using System;

namespace DBRegby.ViewModels
{
    public class RequestManagerViewModel : ViewModelBase
    {
        internal Dictionary<string, string> Keys = new Dictionary<string, string>()
        {
            { "Team", "Team1"},
            { "Team1", "Team"},
            { "Competition1", "Competition"},
            { "Competition", "Competition1"},
        };

        private DataBaseViewModel Db;
        private ObservableCollection<Table> tables;
        private ObservableCollection<Table> requests;
        private ObservableCollection<string> columnList;
        private ObservableCollection<Filter> filters;
        private ObservableCollection<Table> alltables;   
        private ObservableCollection<Filter> groupfilters;              
        private bool checkTableSelect;
        public RequestManagerViewModel(DataBaseViewModel DataBase)
        {

            SelectedColumns = new ObservableCollection<string>();
            columnList = new ObservableCollection<string>();
            SelectedTables = new ObservableCollection<Table>();
            requests = new ObservableCollection<Table>();
            groupfilters = new ObservableCollection<Filter>();
            filters = new ObservableCollection<Filter>();
            ResultTable = new List<Dictionary<string, object?>>();
            JoinedTable = new List<Dictionary<string, object?>>();
            SelectedColumnsTable = new List<Dictionary<string, object?>>();
            FilterHandler = new HandlerFilter(this, "Filters");
            GroupFilterChain = new HandlerFilter(this, "GroupFilters");
            GroupHandler = new HandlerGroup(this);

            Db = DataBase;
            tables = Db.Tables;
            alltables = Db.AllTables;

            FilterHandler.next = GroupHandler;
            GroupHandler.next = GroupFilterChain;
            IsRequestSuccess = false;
            checkTableSelect = true;
        }

        public ObservableCollection<Table> SelectedTables { get; set; }
        public ObservableCollection<Filter> Filters
        {
            get => filters;
            set{this.RaiseAndSetIfChanged(ref filters, value);}
        }
        public ObservableCollection<string> ColumnList
        {
            get => columnList;
            set { this.RaiseAndSetIfChanged(ref columnList, value); }
        }
        public ObservableCollection<Table> Tables
        {
            get => tables;
            set { this.RaiseAndSetIfChanged(ref tables, value); }
        }
        public ObservableCollection<Table> Requests
        {
            get => requests;
            set { this.RaiseAndSetIfChanged(ref requests, value); }
        }

        public DataBaseViewModel DataBase { get; }
        public bool IsRequestSuccess { get; set; }                                
        public bool CheckTableSelect
        {
            get => checkTableSelect;
            set {this.RaiseAndSetIfChanged(ref checkTableSelect, value);}
        }//
        public string? GroupingColumn { get; set; } = null;                         
        public List<Dictionary<string, object?>> ResultTable { get; set; }          
        public List<Dictionary<string, object?>> JoinedTable { get; set; }          
        public List<Dictionary<string, object?>> SelectedColumnsTable { get; set; }

        public ObservableCollection<string> SelectedColumns { get; set; }          

        public ObservableCollection<Filter> GroupFilters
        {
            get => groupfilters;
            set {this.RaiseAndSetIfChanged(ref groupfilters, value);}
        }
   
        public ObservableCollection<Table> AllTables
        {
            get => alltables;
            set {this.RaiseAndSetIfChanged(ref alltables, value);}
        }

        public HandlerFilter FilterHandler { get; set; }                             
        public HandlerGroup GroupHandler { get; set; }                                
        public HandlerFilter GroupFilterChain { get; set; }                    

        public void UpdateColumnList()
        {
            ColumnList = new ObservableCollection<string>();
            if (JoinedTable.Count != 0)
            {
                foreach (var column in JoinedTable[0])
                {
                    ColumnList.Add(column.Key);
                }
            }
            Filters.Clear();
            GroupFilters.Clear();
        }

        public void AddNewRequest(string tableName)
        {
            FilterHandler.Try();
            if (IsRequestSuccess)
            {
                ObservableCollection<string> fields = new ObservableCollection<string>();

                foreach (var item in ResultTable[0])
                {
                    fields.Add(item.Key);
                }

                Requests.Add(new Table(tableName, true, new RequestTableViewModel(ResultTable.ToList()), fields));
                AllTables.Add(Requests.Last());
            }
            CheckTableSelect = true;
            ClearAll();
        }
        public void ClearAll()
        {
            ResultTable.Clear();
            JoinedTable.Clear();
            SelectedColumnsTable.Clear();
            SelectedTables.Clear();
            SelectedColumns.Clear();
            Filters.Clear();
            GroupFilters.Clear();
            ColumnList.Clear();
        }
        private bool checkJoin(string firstKEY, List<Dictionary<string, object?>> table2, string secondKEY)
        {
            try
            {
                JoinedTable = JoinedTable.Join(
                    table2,
                    FirstItem => FirstItem[firstKEY],
                    SecondItem => SecondItem[secondKEY],
                    (FirstItem, SecondItem) => { Dictionary<string, object?> Result = new Dictionary<string, object?>();
                        foreach (var item in FirstItem) Result.TryAdd(item.Key, item.Value);
                        foreach (var item in SecondItem) if (item.Key != secondKEY) Result.TryAdd(item.Key, item.Value);
                        return Result;
                    } ).ToList();
            }
            catch
            {
                return false;
            }
            return true;
        }

        public void JoinTables()
        {
            if (SelectedTables.Count > 0)
            {
                JoinedTable = new List<Dictionary<string, object?>>(SelectedTables[0].tableValues);
                if (SelectedTables.Count > 1)
                {
                    List<Dictionary<string, object?>> NextTable;

                    bool check = false;
                    for (int i = 1; i < SelectedTables.Count; i++)
                    {
                        NextTable = SelectedTables[i].tableValues;
                        foreach (var Keys in Keys)
                        {
                            check = checkJoin(Keys.Key, NextTable, Keys.Value);
                            if (check) break;
                            else
                            {
                                check = checkJoin(Keys.Value, NextTable, Keys.Key);
                                if (check) break;
                            }
                        }
                        if (!check)
                        {
                            JoinedTable.Clear();
                            ResultTable = JoinedTable;
                            UpdateColumnList();
                            return;
                        }
                    }
                }
                UpdateColumnList();
                ResultTable = JoinedTable;
            }
            else
            {
                JoinedTable.Clear();
                ResultTable = JoinedTable;
                ColumnList.Clear();
                CheckTableSelect = true;
            }
        }
        public void Selected()
        {
            SelectedColumnsTable = JoinedTable.Select(item =>
            {
                return new Dictionary<string, object?>(item.Where(property => SelectedColumns.Any(column => column == property.Key)));
            }).ToList();
            ResultTable = SelectedColumnsTable;
        }

    }

}

