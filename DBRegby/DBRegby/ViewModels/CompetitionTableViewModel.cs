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
    internal class CompetitionTableViewModel : ViewModelBase
    {
        private ObservableCollection<Competition> table;
        public ObservableCollection<Competition> thisTable
        {
            get { return table; }
            set { table = value; }
        }
        public CompetitionTableViewModel(ObservableCollection<Competition> Collection)
        {
            thisTable = Collection;
        }

        public override ObservableCollection<Competition> getThisTable()
        {
            return thisTable;
        }
    }
}
