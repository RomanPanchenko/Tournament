using System.Collections.Generic;
using TournamentWebApi.BLL.Models;

namespace TournamentWebApi.BLL.Generators.Interfaces
{
    public interface IMatchesHelper
    {
        /// <summary>
        ///     Get generated match model where given player will take part for next round based on players list and previous
        ///     matches
        /// </summary>
        /// <param name="player">Player taking part in this match</param>
        /// <param name="playersToProcess">Candidates to play with given player</param>
        /// <param name="previousMatches">Previous matches</param>
        /// <returns>Generated match model</returns>
        MatchModel GetMatchModelForNextRound(PlayerModel player, IEnumerable<PlayerModel> playersToProcess, List<MatchModel> previousMatches);
    }
}