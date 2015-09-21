using System.Collections.Generic;
using TournamentWebApi.BLL.Models;

namespace TournamentWebApi.BLL.Interfaces
{
    public interface IMatchGenerator
    {
        /// <summary>
        ///     Gets matches for next round in this tournament
        /// </summary>
        /// <param name="players">List of all players taking part in this tournament</param>
        /// <param name="previousMatches">All matches were played in this tournament</param>
        /// <returns>MatchModel entities collection for the next tournament round. Player pairs and their colors.</returns>
        IEnumerable<MatchModel> GetMatchesForNextRound(IEnumerable<PlayerModel> players, IEnumerable<MatchModel> previousMatches);

        /// <summary>
        ///     Gets matches with assigned players result randomly assigned
        /// </summary>
        /// <param name="roundMatches">List of matches in the last round</param>
        /// <returns>MatchModel entities collection for the next tournament round. Players result are randomly assigned.</returns>
        IEnumerable<MatchModel> AssignRandomResultsForGeneratedMatches(IEnumerable<MatchModel> roundMatches);
    }
}
