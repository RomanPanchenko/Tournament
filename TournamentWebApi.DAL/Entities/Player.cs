using System.Collections.Generic;
using TournamentWebApi.Core.Entities;

namespace TournamentWebApi.DAL.Entities
{
    public class Player : PlayerEntity
    {
        public virtual ICollection<Match> Player1Matches { get; set; }
        public virtual ICollection<Match> Player2Matches { get; set; }
        public virtual ICollection<Match> WinnerMatches { get; set; }
    }
}
