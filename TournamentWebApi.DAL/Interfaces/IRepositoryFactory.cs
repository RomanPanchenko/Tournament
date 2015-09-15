namespace TournamentWebApi.DAL.Interfaces
{
    public interface IRepositoryFactory
    {
        IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
    }
}
