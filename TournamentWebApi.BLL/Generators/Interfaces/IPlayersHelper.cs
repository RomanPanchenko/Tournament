using System.Collections.Generic;
using TournamentWebApi.BLL.Models;
using TournamentWebApi.Core.Enums;

namespace TournamentWebApi.BLL.Generators.Interfaces
{
    public interface IPlayersHelper
    {
        /// <summary>
        ///     Wheather players can play together
        /// </summary>
        /// <param name="player1">Player 1</param>
        /// <param name="player2">Player 2</param>
        /// <param name="previousMatches">Played matches for all players</param>
        /// <returns>bool</returns>
        bool PlayersCanPlayTogether(PlayerModel player1, PlayerModel player2, IList<MatchModel> previousMatches);

        /// <summary>
        ///     Gets list op players with calculated rating for each player
        /// </summary>
        /// <param name="players">Players to process</param>
        /// <param name="previousMatches">Played matches for all players</param>
        /// <returns>List of players</returns>
        IEnumerable<PlayerModel> GetPlayersWithCalculatedRating(IList<PlayerModel> players, IList<MatchModel> previousMatches);

        /// <summary>
        ///     Whether player can play with given color
        /// </summary>
        /// <param name="player">Player to process</param>
        /// <param name="color">Defined color</param>
        /// <param name="previousMatches">Played matches for given player</param>
        /// <returns>bool</returns>
        bool PlayerCanPlayWithDefinedColor(PlayerModel player, ChessColor color, IEnumerable<MatchModel> previousMatches);
    }
}