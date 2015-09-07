using System;
using TournamentWebApi.DAL.Entities;

namespace TournamentWebApi.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        ///     Returns MatchRepository
        /// </summary>
        IGenericRepository<Match> MatchRepository { get; }

        /// <summary>
        ///     Returns PlayerRepository
        /// </summary>
        IGenericRepository<Player> PlayerRepository { get; }
    }
}