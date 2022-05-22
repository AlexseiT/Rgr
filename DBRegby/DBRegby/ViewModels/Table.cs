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
        public Dictionary<string, List<object?>> tableValues { get; }
        public ObservableCollection<string> fields { get; set; }
        private ViewModelBase thisTableView;

        public Table(string title, bool checkTable, ViewModelBase thisTableView, ObservableCollection<string> fields)
        {

            this.title = title;

            this.checkTable = checkTable;

            this.thisTableView = thisTableView;

            this.fields = fields;

            tableValues = new Dictionary<string, List<object?>>();

            dynamic myTable = TableView.getThisTable();

            if (myTable != null)
            {
                foreach (string field in this.fields)
                {
                    tableValues.Add(field, new List<object?>() { this.title + ": " + field });
                }
                for (int i = 0; i < tableValues.Count; i++)
                {
                    foreach (string field in this.fields)
                    {
                        for (int j = 0; j < myTable.Count; j++)
                        {
                            tableValues[field].Add(myTable[j][field]);
                        }
                    }
                }
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
    }
}
