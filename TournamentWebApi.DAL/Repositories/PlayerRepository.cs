using NHibernate;
using System.Linq;
using TournamentWebApi.DAL.Entities;

namespace TournamentWebApi.DAL.Repositories
{
    public class PlayerRepository : GenericRepository<Player>
    {
        public PlayerRepository(ISessionFactory sessionFactory)
            : base(sessionFactory)
        {

        }

        protected override IQueryable<Player> All(ISession session)
        {
            return base.All(session).Where(p => p.PlayerId > 0);
        }
    }
}
