using System.Linq;
using NHibernate;
using NHibernate.Linq;
using TournamentWebApi.DAL.Entities;

namespace TournamentWebApi.DAL.Repositories
{
    public class RoleRepository : GenericRepository<Role>
    {
        public RoleRepository(ISessionFactory sessionFactory)
            : base(sessionFactory)
        {
        }

        protected override IQueryable<Role> All(ISession session)
        {
            return base.All(session)
                .Fetch(p => p.Accounts);
        }
    }
}
