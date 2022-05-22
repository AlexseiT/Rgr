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
    internal class DataBaseViewModel : ViewModelBase
    {
        private ObservableCollection<Team> CollectionTeams;
        private ObservableCollection<Game> CollectionGames;
        private ObservableCollection<Table> CollectionTables;
        private ObservableCollection<Player> CollectionPlayers;
        private ObservableCollection<Competition> CollectionCompetitions;
        private ObservableCollection<CompetitionTeam> CollectionCompetitionsTeams;
        public ObservableCollection<Table> Tables
        {
            get => CollectionTables;
            set{this.RaiseAndSetIfChanged(ref CollectionTables, value);}
        }

        public ObservableCollection<Team> Teams
        {
            get => CollectionTeams;
            set{this.RaiseAndSetIfChanged(ref CollectionTeams, value);}
        }
        public ObservableCollection<Game> Games
        {
            get => CollectionGames;
            set{this.RaiseAndSetIfChanged(ref CollectionGames, value);}
        }

        public ObservableCollection<Competition> Competitions
        {
            get => CollectionCompetitions;set{this.RaiseAndSetIfChanged(ref CollectionCompetitions, value);}
        }

        public ObservableCollection<CompetitionTeam> CompetitionsTeams
        {
            get => CollectionCompetitionsTeams;
            set{this.RaiseAndSetIfChanged(ref CollectionCompetitionsTeams, value);}
        }

        public ObservableCollection<Player> Players
        {
            get => CollectionPlayers;
            set{this.RaiseAndSetIfChanged(ref CollectionPlayers, value);}
        }
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
                            result.Add(fieldsList[i].Remove(fieldsList[i].IndexOf("(")));
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
                CollectionTables = new ObservableCollection<Table>();
                var DataBase = new RegbyDataBaseContext();

                string tableInfo = DataBase.Model.ToDebugString();
                tableInfo = tableInfo.Replace(" ", "");

                string[] splitTableInfo = tableInfo.Split("\r\n");

                List<string> fieldlist = new List<string>(splitTableInfo); // Fix bug 

            CollectionCompetitions = new ObservableCollection<Competition>(DataBase.Competitions);
                CollectionCompetitionsTeams = new ObservableCollection<CompetitionTeam>(DataBase.CompetitionTeams);
                CollectionGames = new ObservableCollection<Game>(DataBase.Games);
                CollectionPlayers = new ObservableCollection<Player>(DataBase.Players);
                CollectionTeams = new ObservableCollection<Team>(DataBase.Teams);

                CollectionTables.Add(new Table("Competitions", false, new CompetitionTableViewModel(CollectionCompetitions), searchField("Competition", fieldlist)));
                CollectionTables.Add(new Table("CompetitionsTeams", false, new CompetitionTeamTableViewModel(CollectionCompetitionsTeams), searchField("CompetitionTeam", fieldlist)));
                CollectionTables.Add(new Table("Games", false, new GameTableViewModel(CollectionGames), searchField("Game", fieldlist)));
                CollectionTables.Add(new Table("Players", false, new PlayerTableViewModel(CollectionPlayers), searchField("Player", fieldlist)));
                CollectionTables.Add(new Table("Teams", false, new TeamTableViewModel(CollectionTeams), searchField("Team", fieldlist)));
            }

        }
    }