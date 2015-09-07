using NHibernate;
using NHibernate.Linq;
using System.Linq;

namespace TournamentWebApi.DAL.Wrappers
{
    public class NhSession<TEntity> : INhSession<TEntity>
    {
        private readonly ISession _session;

        public NhSession(ISession session)
        {
            _session = session;
        }

        public IQueryable<TEntity> Query()
        {
            return _session.Query<TEntity>();
        }
    }
}
