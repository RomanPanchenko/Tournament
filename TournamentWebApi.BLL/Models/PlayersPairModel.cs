using System;
using TournamentWebApi.Core.Entities;

namespace TournamentWebApi.BLL.Models
{
    public class PlayersPairModel : Entity
    {
        public PlayerModel Player1 { get; set; }
        public PlayerModel Player2 { get; set; }
        public DateTime MatchStartTime { get; set; }
        public int Round { get; set; }
    }
}
