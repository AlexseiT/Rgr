using System;
using System.Collections.Generic;

namespace DBRegby.Models
{
    public partial class Game
    {
        public double Id { get; set; }
        public string? Competition { get; set; }
        public string? Home { get; set; }
        public string? Score { get; set; }
        public string? Away { get; set; }
        public string? Venue { get; set; }
        public object? this[string field]
        {
            get
            {
                switch (field)
                {
                    case "Id": return Id;
                    case "Competition": return Competition;
                    case "Home": return Home;
                    case "Score": return Score;
                    case "Away": return Away;
                    case "Venue": return Venue;
                }
                return null;
            }
        }
        public virtual Competition? CompetitionNavigation { get; set; }
    }
}
