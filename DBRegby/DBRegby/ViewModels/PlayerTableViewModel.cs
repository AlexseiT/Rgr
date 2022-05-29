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
    internal class PlayerTableViewModel : ViewModelBase
    {
        private ObservableCollection<Player> table;
        public ObservableCollection<Player> thisTable
        {
            get { return table; }
            set { table = value; }
        }
        RegbyDataBaseContext DataBase;
        public PlayerTableViewModel(ObservableCollection<Player> Collection, RegbyDataBaseContext DataBase)
        {
            this.DataBase = DataBase;
            thisTable = Collection;
            Item = new object();
        }

        public override ObservableCollection<Player> getThisTable()
        {
            return thisTable;
        }
        public void Save()
        {
            DataBase.SaveChanges();
        }

        public void AddField()
        {
            thisTable.Add(new Player { Id = 0});
        }
    }
}
