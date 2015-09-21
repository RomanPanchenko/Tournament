using TournamentWebApi.Core.Entities;
using TournamentWebApi.Core.Enums;

namespace TournamentWebApi.BLL.Models
{
    public class PlayerModel : PlayerEntity
    {
        /// <summary>
        ///     Flag that defines that this player is winner
        /// </summary>
        public bool Winner { get; set; }

        /// <summary>
        ///     Chess color
        /// </summary>
        public ChessColor ChessColor { get; set; }
    }
}