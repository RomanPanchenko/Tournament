using System;
using TournamentWebApi.BLL.Models;
using TournamentWebApi.Core.Entities;
using TournamentWebApi.Core.Enums;

namespace TournamentWebApi.WEB.ViewModels
{
    public class MatchViewModel : MatchEntity
    {
        public PlayerModel Player1 { get; set; }
        public PlayerModel Player2 { get; set; }
        public PlayerModel Winner { get; set; }
        public DateTime MatchDateTime { get; set; }
        public ChessColor PlayerColor1 { get; set; }
        public ChessColor PlayerColor2 { get; set; }
    }
}