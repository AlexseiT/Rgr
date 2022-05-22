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
    internal class TeamTableViewModel : ViewModelBase
    {
        private ObservableCollection<Team> table;
        public ObservableCollection<Team> thisTable
        {
            get {return table;}
            set {table = value;}
        }
        public TeamTableViewModel(ObservableCollection<Team> Collection)
        {
            thisTable = Collection;
        }

        public override ObservableCollection<Team> getThisTable()
        {
            return thisTable;
        }
    }
}
