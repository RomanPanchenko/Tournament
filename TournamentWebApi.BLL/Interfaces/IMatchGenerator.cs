using System.Collections.Generic;
using TournamentWebApi.BLL.Models;

namespace TournamentWebApi.BLL.Interfaces
{
    public interface IMatchGenerator
    {
        /// <summary>
        /// Gets matches for next round in this tournament
        /// </summary>
        /// <param name="players">List of all players taking part in this tournament</param>
        /// <param name="pastMatches">All matches were played in this tournament</param>
        /// <returns>MatchModel entities collection for next math round. Player pairs and their colors.</returns>
        IEnumerable<MatchModel> GetMatchesForNextRound(IEnumerable<PlayerModel> players, IEnumerable<MatchModel> pastMatches);
    }
}
