using TournamentWebApi.Core.Entities;

namespace TournamentWebApi.DAL.Entities
{
    public class Match : MatchEntity
    {
        public virtual int PlayerId1 { get; set; }
        public virtual int PlayerId2 { get; set; }
        public virtual int WinnerId { get; set; }

        public virtual Player PlayerForeignKey1 { get; set; }
        public virtual Player PlayerForeignKey2 { get; set; }
        public virtual Player PlayerForeignKey3 { get; set; }
    }
}
