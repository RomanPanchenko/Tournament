using System.Collections.Generic;
using TournamentWebApi.Core.Entities;
using TournamentWebApi.Core.Enums;

namespace TournamentWebApi.BLL.Models
{
    public class PlayerModel : PlayerEntity
    {
        /// <summary>
        ///     Flag that indicates that we shouldn't change this color except last mathes situation
        /// </summary>
        public bool StronglyPreferredColor { get; set; }

        /// <summary>
        ///     Flag that defines that this player is winner
        /// </summary>
        public bool Winner { get; set; }

        /// <summary>
        ///     Chess color
        /// </summary>
        public ChessColor ChessColor { get; set; }

        /// <summary>
        ///     List of opponents ids
        /// </summary>
        public List<int> OponentsIds { get; set; }

        /// <summary>
        ///     Flag that indicates that this player has an opponent
        /// </summary>
        public bool HasPair { get; set; }
    }
}