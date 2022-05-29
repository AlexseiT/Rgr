using System;
using System.Collections.Generic;

namespace DBRegby.Models
{
    public partial class Competition
    {
        public Competition()
        {
            CompetitionTeams = new HashSet<CompetitionTeam>();
            GamesNavigation = new HashSet<Game>();
        }

        public string Competition1 { get; set; } = null!;
        public long? Teams { get; set; }
        public long? Games { get; set; }
        public string StartDate { get; set; } = null!;
        public string EndDate { get; set; } = null!;
        public object? this[string field]
        {
            get
            {
                switch (field)
                {
                    case "Competition1": return Competition1;
                    case "Teams": return Teams;
                    case "Games": return Games;
                    case "StartDate": return StartDate;
                    case "EndDate": return EndDate;
                }
                return null;
            }
        }
        public string Key()
        {
            return "Competition1";
        }
        public virtual ICollection<CompetitionTeam> CompetitionTeams { get; set; }
        public virtual ICollection<Game> GamesNavigation { get; set; }
    }
}
