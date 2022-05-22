using System;
using System.Collections.Generic;

namespace DBRegby.Models
{
    public partial class CompetitionTeam
    {
        public string Competition { get; set; } = null!;
        public string Team { get; set; } = null!;
        public long? Result { get; set; }
        public object? this[string field]
        {
            get
            {
                switch (field)
                {
                    case "Competition": return Competition;
                    case "Team": return Team;
                    case "Result": return Result;
                }
                return null;
            }
        }
        public virtual Competition CompetitionNavigation { get; set; } = null!;
        public virtual Team TeamNavigation { get; set; } = null!;
    }
}
