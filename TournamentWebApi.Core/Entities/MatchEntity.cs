using System;

namespace TournamentWebApi.Core.Entities
{
    public abstract class MatchEntity : Entity
    {
        public virtual int MatchId { get; set; }
        public virtual DateTime MatchStartTime { get; set; }
        public virtual bool Player1PlaysWhite { get; set; }
        public virtual int Round { get; set; }
    }
}
