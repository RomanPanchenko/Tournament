using NHibernate;
using System.Collections.Generic;
using System.Linq;
using TournamentWebApi.DAL.Entities;
using TournamentWebApi.DAL.Repositories;

namespace TournamentWebApi.DAL.Tests.AuxiliaryClasses
{
    internal class PlayerRepository : GenericRepository<Player>
    {
        public List<Player> Players { get; set; }

        public PlayerRepository(ISessionFactory sessionFactory)
            : base(sessionFactory)
        {
        }

        protected override IQueryable<Player> All(ISession session)
        {
            return Players.AsQueryable();
        }
    }
}
