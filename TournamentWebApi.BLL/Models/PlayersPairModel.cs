using System;

namespace TournamentWebApi.BLL.Models
{
    public class PlayersPairModel
    {
        public PlayerModel Player1 { get; set; }
        public PlayerModel Player2 { get; set; }
        public DateTime MatchStartTime { get; set; }
        public int Round { get; set; }
    }
}
