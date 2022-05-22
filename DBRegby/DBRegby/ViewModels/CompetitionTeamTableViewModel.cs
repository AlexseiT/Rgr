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
    internal class CompetitionTeamTableViewModel : ViewModelBase
    {
        private ObservableCollection<CompetitionTeam> table;
        public ObservableCollection<CompetitionTeam> thisTable
        {
            get { return table; }
            set { table = value; }
        }
        public CompetitionTeamTableViewModel(ObservableCollection<CompetitionTeam> Collection)
        {
            thisTable = Collection;
        }

        public override ObservableCollection<CompetitionTeam> getThisTable()
        {
            return thisTable;
        }
    }
}
