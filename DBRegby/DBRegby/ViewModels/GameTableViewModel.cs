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
    internal class GameTableViewModel : ViewModelBase
    {
        private ObservableCollection<Game> table;
        public ObservableCollection<Game> thisTable
        {
            get { return table; }
            set { table = value; }
        }
        public GameTableViewModel(ObservableCollection<Game> Collection)
        {
            thisTable = Collection;
        }

        public override ObservableCollection<Game> getThisTable()
        {
            return thisTable;
        }
    }
}
