using NHibernate;
using Ninject;
using TournamentWebApi.DAL.Entities;
using TournamentWebApi.DAL.Interfaces;

namespace TournamentWebApi.DAL.UnitsOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private bool _disposed;

        [Inject]
        public ISession Session { get; set; }

        [Inject]
        public IGenericRepository<Match> MatchRepository { get; set; }

        [Inject]
        public IGenericRepository<Player> PlayerRepository { get; set; }

        public void Dispose()
        {
            if (!_disposed)
            {
                if (Session.IsOpen)
                {
                    Session.Close();
                    Session.Dispose();
                }

                _disposed = true;
            }
        }

        ~UnitOfWork()
        {
            Dispose();
        }
    }
}