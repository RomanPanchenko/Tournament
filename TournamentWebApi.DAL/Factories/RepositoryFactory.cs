using System;
using System.Collections.Generic;
using NHibernate;
using TournamentWebApi.DAL.Entities;
using TournamentWebApi.DAL.Interfaces;
using TournamentWebApi.DAL.Repositories;

namespace TournamentWebApi.DAL.Factories
{
    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly Dictionary<Type, Lazy<object>> _repositories;

        public RepositoryFactory(ISessionFactory sessionFactory)
        {
            _repositories = new Dictionary<Type, Lazy<object>>
            {
                {typeof (Account), new Lazy<object>(() => new AccountRepository(sessionFactory))},
                {typeof (Match), new Lazy<object>(() => new MatchRepository(sessionFactory))},
                {typeof (Player), new Lazy<object>(() => new PlayerRepository(sessionFactory))},
                {typeof (Role), new Lazy<object>(() => new RoleRepository(sessionFactory))}
            };
        }

        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            return (IGenericRepository<TEntity>)_repositories[typeof(TEntity)].Value;
        }
    }
}