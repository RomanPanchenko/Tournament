using System.Linq;

namespace TournamentWebApi.DAL.Wrappers
{
    public interface INhSession<out TEntity>
    {
        IQueryable<TEntity> Query();
    }
}
