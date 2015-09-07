using System.Collections.Generic;
using TournamentWebApi.BLL.Models;

namespace TournamentWebApi.BLL.Interfaces
{
    public interface IPlayerService
    {
        /// <summary>
        ///     Gets specified player from the database by PlayerId.
        /// </summary>
        /// <param name="playerId">Player id</param>
        PlayerModel Get(int playerId);

        /// <summary>
        ///     Gets player list fromn the database.
        /// </summary>
        IEnumerable<PlayerModel> GetAllPlayers();

        /// <summary>
        ///     Gets player score
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="matchService"></param>
        /// <returns></returns>
        ScoreModel GetPlayerScore(int playerId, IMatchService matchService);

        /// <summary>
        ///     Gets scores for all players
        /// </summary>
        /// <param name="matchService"></param>
        IEnumerable<ScoreModel> GetAllPlayersScore(IMatchService matchService);

        /// <summary>
        ///     Gets all matches for specified player
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="matchService"></param>
        /// <returns></returns>
        IEnumerable<MatchModel> GetMatches(int playerId, IMatchService matchService);
    }
}