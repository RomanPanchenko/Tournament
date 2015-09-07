using TournamentWebApi.Core.Entities;

namespace TournamentWebApi.BLL.Models
{
    public class MatchModel : MatchEntity
    {
        public PlayerModel Player1 { get; set; }
        public PlayerModel Player2 { get; set; }
        public PlayerModel Winner { get; set; }
    }
}
