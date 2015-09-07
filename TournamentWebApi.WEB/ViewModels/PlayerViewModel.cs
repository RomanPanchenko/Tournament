using System.Collections.Generic;
using TournamentWebApi.BLL.Models;
using TournamentWebApi.Core.Entities;
using TournamentWebApi.Core.Enums;

namespace TournamentWebApi.WEB.ViewModels
{
    public class PlayerViewModel : PlayerEntity
    {
        public ChessColor ChessColor { get; set; }
        public ICollection<MatchModel> Matches { get; set; }
    }
}