using TournamentWebApi.Core.Entities;

namespace TournamentWebApi.BLL.Models
{
    public class ScoreModel : PlayerEntity
    {
        /// <summary>
        ///     How many points player has
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        ///     Player position in rate table according to its score
        /// </summary>
        public int Position { get; set; }
    }
}