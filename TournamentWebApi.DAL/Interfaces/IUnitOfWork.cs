using TournamentWebApi.DAL.Entities;

namespace TournamentWebApi.DAL.Interfaces
{
    public interface IUnitOfWork
    {
        /// <summary>
        ///     Returns AccountRepository
        /// </summary>
        IGenericRepository<Account> AccountRepository { get; }

        /// <summary>
        ///     Returns RoleRepository
        /// </summary>
        IGenericRepository<Role> RoleRepository { get; }

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