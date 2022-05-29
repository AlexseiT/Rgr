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
using System.Collections.Specialized;

namespace DBRegby.ViewModels
{
    internal class GameTableViewModel : ViewModelBase
    {
        private ObservableCollection<Game> table;
        public ObservableCollection<Game> thisTable
        {
            get { return table; }
            set { table = value; }
        }
        RegbyDataBaseContext DataBase;
        public GameTableViewModel(ObservableCollection<Game> Collection, RegbyDataBaseContext DataBase)
        {
            this.DataBase = DataBase;
            thisTable = Collection;
            Item = new object();
        }

        public override ObservableCollection<Game> getThisTable()
        {
            return thisTable;
        }
       
        public void Save()
        {
            DataBase.SaveChanges();
        }

        public void AddField()
        {
            thisTable.Add(new Game { Id = 0 });
        }
    }
}
