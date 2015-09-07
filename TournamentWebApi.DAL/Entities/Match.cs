using TournamentWebApi.Core.Entities;

namespace TournamentWebApi.DAL.Entities
{
    public class Match : MatchEntity
    {
        public virtual Player Player1 { get; set; }
        public virtual Player Player2 { get; set; }
        public virtual Player Winner { get; set; }
    }
}
