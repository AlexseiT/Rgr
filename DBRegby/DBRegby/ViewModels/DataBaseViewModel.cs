using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using ReactiveUI;
using System.Reactive;
using DBRegby.Models;
using Microsoft.Data.Sqlite;
using System.IO;
using System;
using Microsoft.EntityFrameworkCore;

namespace DBRegby.ViewModels
{
    public class DataBaseViewModel : ViewModelBase
    {
        private ObservableCollection<Team> CollectionTeams;
        private ObservableCollection<Game> CollectionGames;
        private ObservableCollection<Table> CollectionAllTables;  
        private ObservableCollection<Table> CollectionTables;
        private ObservableCollection<Table> CollectionRequest;
        private ObservableCollection<Player> CollectionPlayers;
        private ObservableCollection<Competition> CollectionCompetitions;
        private ObservableCollection<CompetitionTeam> CollectionCompetitionsTeams;
        public RegbyDataBaseContext DataBase = new RegbyDataBaseContext();
        public ObservableCollection<Table> AllTables
        {
            get => CollectionAllTables;
            set {this.RaiseAndSetIfChanged(ref CollectionAllTables, value);}
        }
        public ObservableCollection<Table> Tables
        {
            get => CollectionTables;
            set { this.RaiseAndSetIfChanged(ref CollectionTables, value); }
        }
        public ObservableCollection<Team> Teams
        {
            get => CollectionTeams;
            set { this.RaiseAndSetIfChanged(ref CollectionTeams, value); }
        }
        public ObservableCollection<Game> Games
        {
            get => CollectionGames;
            set { this.RaiseAndSetIfChanged(ref CollectionGames, value); }
        }
        public ObservableCollection<Competition> Competitions
        {
            get => CollectionCompetitions; set { this.RaiseAndSetIfChanged(ref CollectionCompetitions, value); }
        }
        public ObservableCollection<CompetitionTeam> CompetitionsTeams
        {
            get => CollectionCompetitionsTeams;
            set { this.RaiseAndSetIfChanged(ref CollectionCompetitionsTeams, value); }
        }
        public ObservableCollection<Player> Players
        {
            get => CollectionPlayers;
            set { this.RaiseAndSetIfChanged(ref CollectionPlayers, value); }
        }
        public ObservableCollection<Table> Requests
        {
            get => CollectionRequest;
            set{this.RaiseAndSetIfChanged(ref CollectionRequest, value);}
        }
        public List<Table> SelectedTables { get; set; }
        private ObservableCollection<string> searchField(string eTitle, List<string> fieldsList)
        {
            ObservableCollection<string> result = new ObservableCollection<string>();
            for (int i = 0; i < fieldsList.Count(); i++)
            {
                if (fieldsList[i].IndexOf("EntityType:" + eTitle) != -1)
                {
                    try
                    {
                        i++;
                        while (fieldsList[i].IndexOf("(") != -1 && i < fieldsList.Count())
                        {
                            string field = fieldsList[i].Remove(fieldsList[i].IndexOf("("));

                            if (eTitle == "Teams" && field == "Team1")
                                result.Add("Team");

                            else if (eTitle == "Competitions" && field == "Competition1")
                                result.Add("Competition");

                            else if (eTitle == "CompetitionsTeams" && field == "Competition")
                                result.Add("Competition");

                            else if (eTitle == "CompetitionsTeams" && field == "Team")
                                result.Add("Team");
                            else
                                result.Add(field);

                            i++;
                        }
                        return result;
                    }
                    catch
                    {
                        return result;
                    }
                }
            }
            return result;
        }
        public DataBaseViewModel()
        {
            try
            {
                CollectionTables = new ObservableCollection<Table>();
                CollectionRequest = new ObservableCollection<Table>();

                string tableInfo = DataBase.Model.ToDebugString();
                tableInfo = tableInfo.Replace(" ", "");
                string[] splitTableInfo = tableInfo.Split("\r\n");

                List<string> fieldlist = new List<string>(splitTableInfo.Where(str => str.IndexOf("Entity") != -1 || (str.IndexOf("(") != -1 && str.IndexOf("<") == -1) && str.IndexOf("Navigation") == -1));

                DataBase.Competitions.Load<Competition>();
                CollectionCompetitions = DataBase.Competitions.Local.ToObservableCollection();

                DataBase.Players.Load<Player>();
                CollectionPlayers = DataBase.Players.Local.ToObservableCollection();

                DataBase.Games.Load<Game>();
                CollectionGames = DataBase.Games.Local.ToObservableCollection();

                DataBase.Teams.Load<Team>();
                CollectionTeams = DataBase.Teams.Local.ToObservableCollection();

                DataBase.CompetitionTeams.Load<CompetitionTeam>();
                CollectionCompetitionsTeams = DataBase.CompetitionTeams.Local.ToObservableCollection();

                CollectionTables.Add(new Table("Competitions", false, new CompetitionTableViewModel(CollectionCompetitions, DataBase), searchField("Competition", fieldlist)));
                CollectionTables.Add(new Table("CompetitionsTeams", false, new CompetitionTeamTableViewModel(CollectionCompetitionsTeams, DataBase), searchField("CompetitionTeam", fieldlist)));
                CollectionTables.Add(new Table("Games", false, new GameTableViewModel(CollectionGames, DataBase), searchField("Game", fieldlist)));
                CollectionTables.Add(new Table("Players", false, new PlayerTableViewModel(CollectionPlayers, DataBase), searchField("Player", fieldlist)));
                CollectionTables.Add(new Table("Teams", false, new TeamTableViewModel(CollectionTeams, DataBase), searchField("Team", fieldlist)));

                AllTables = new ObservableCollection<Table>(Tables.ToList());
            }
            catch
            {

            }
        }
        public string CurrentTableName { get; set; }
        public void DeleteSelectedItem()
        {
            Table SelectTable = Tables.Where(table => table.Title == CurrentTableName).ToList()[0];
            object? Item = SelectTable.getItem();
            if (Item != null)
            {
                switch (CurrentTableName)
                {
                    case "Competitions":
                        {
                            Competitions.Remove(Item as Competition);
                            break;
                        }
                    case "CompetitionsTeams":
                        {
                            CompetitionsTeams.Remove(Item as CompetitionTeam);
                            break;
                        }
                    case "Games":
                        {
                            Games.Remove(Item as Game);
                            break;
                        }
                    case "Players":
                        {
                            Players.Remove(Item as Player);
                            break;
                        }
                    case "Teams":
                        {
                            Teams.Remove(Item as Team);
                            break;
                        }
                }
            }
        }
    }
}