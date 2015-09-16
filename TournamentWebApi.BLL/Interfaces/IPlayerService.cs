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
        /// <returns>PlayerModel entity</returns>
        PlayerModel Get(int playerId);

        /// <summary>
        ///     Gets player list fromn the database.
        /// </summary>
        /// <returns>PlayerModel entities collection</returns>
        IEnumerable<PlayerModel> GetAllPlayers();
    }
}