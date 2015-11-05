using NHibernate;
using TournamentWebApi.DAL.Entities;

namespace TournamentWebApi.DAL.Repositories
{
    public class MatchRepository : GenericRepository<Match>
    {
        public MatchRepository(ISessionFactory sessionFactory)
            : base(sessionFactory)
        {
        }
    }
}
