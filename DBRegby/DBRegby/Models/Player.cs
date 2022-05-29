using System;
using System.Collections.Generic;

namespace DBRegby.Models
{
    public partial class Player
    {
        public double Id { get; set; }
        public string? Team { get; set; }
        public string? Players { get; set; }
        public long? Appearances { get; set; }
        public long? TestCaps { get; set; }
        public long? Tries { get; set; }
        public long? Conversions { get; set; }
        public long? Penalties { get; set; }
        public long? DropGoals { get; set; }
        public long? PointsFor { get; set; }

        public object? this[string field]
        {
            get
            {
                switch (field)
                {
                    case "Id": return Id;
                    case "Team": return Team;
                    case "Player": return Players;
                    case "Appearances": return Appearances;
                    case "TestCaps": return TestCaps;
                    case "Tries": return Tries;
                    case "Conversions": return Conversions;
                    case "Penalties": return Penalties;
                    case "DropGoals": return DropGoals;
                    case "PointsFor": return PointsFor;
                }
                return null;
            }
        }
        public string Key()
        {
            return "Id";
        }
        public virtual Team? TeamNavigation { get; set; }
    }
}
