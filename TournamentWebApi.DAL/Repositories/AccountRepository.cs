using System.Linq;
using NHibernate;
using NHibernate.Linq;
using TournamentWebApi.DAL.Entities;

namespace TournamentWebApi.DAL.Repositories
{
    public class AccountRepository : GenericRepository<Account>
    {
        public AccountRepository(ISessionFactory sessionFactory)
            : base(sessionFactory)
        {
        }

        protected override IQueryable<Account> All(ISession session)
        {
            return base.All(session)
                .Fetch(p => p.Roles);
        }
    }
}
