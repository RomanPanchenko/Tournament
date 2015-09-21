using NHibernate;
using NHibernate.Linq;
using System.Linq;
using TournamentWebApi.DAL.Entities;

namespace TournamentWebApi.DAL.Repositories
{
    public class MatchRepository : GenericRepository<Match>
    {
        public MatchRepository(ISessionFactory sessionFactory)
            : base(sessionFactory)
        {

        }

        protected override IQueryable<Match> All(ISession session)
        {
            return base.All(session)
                .Fetch(p => p.Player1)
                .Fetch(p => p.Player2)
                .Fetch(p => p.Winner);
        }
    }
}
