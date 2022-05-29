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
    public class Table
    {
        private string title;
        public bool checkTable { get; }
        public List<Dictionary<string, object?>> tableValues { get; }
        public string Key { get; set; }

        private ViewModelBase thisTableView;
        public Table(string title, bool checkTable, ViewModelBase ThisTableView, ObservableCollection<string> Fields)
        {

            this.title = title;      
            this.checkTable = checkTable;
            thisTableView = ThisTableView;
            fields = Fields;
            tableValues = new List<Dictionary<string, object?>>();
            dynamic myTable = TableView.getThisTable();

            if (myTable != null)
            {
                Key = myTable[0].Key();
                for (int j = 0; j < myTable.Count; j++)
                {
                    Dictionary<string, object?> tmp = new Dictionary<string, object?>();
                    foreach (string prop in fields)
                    {
                        tmp.Add(prop, myTable[j][prop]);
                    }
                    tableValues.Add(tmp);
                }
            }
            else if (checkTable)
            {
                  tableValues = TableView.getRowsThisTable();
            }
        }
        public string Title
        {
            get {return title;}
            set {title = value;}
        }
        public ViewModelBase TableView
        {
            get {return thisTableView;}
            set {thisTableView = value;}
        }
        public ObservableCollection<string> fields { get; set; }
        public object? getItem()
        {
            return TableView.Item;
        }

    }   
}
