using System;
using System.Collections.Generic;

namespace DBRegby.Models
{
    public partial class Team
    {
        public Team()
        {
            CompetitionTeams = new HashSet<CompetitionTeam>();
            Players = new HashSet<Player>();
        }

        public string? Country { get; set; } = null!;
        public string? Team1 { get; set; } = null!;
        public object? this[string field]
        {
            get
            {
                switch (field)
                {
                    case "Country": return Country;
                    case "Team1": return Team1;
                }
                return null;
            }
        }
        public string Key()
        {
            return "Team1";
        }
        public virtual ICollection<CompetitionTeam> CompetitionTeams { get; set; }
        public virtual ICollection<Player> Players { get; set; }
    }
}
