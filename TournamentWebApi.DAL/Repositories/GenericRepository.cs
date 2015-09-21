using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using NHibernate;
using NHibernate.Linq;
using TournamentWebApi.Core.Enums;
using TournamentWebApi.DAL.Extensions;
using TournamentWebApi.DAL.Interfaces;

namespace TournamentWebApi.DAL.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly ISessionFactory _sessionFactory;

        public GenericRepository(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public virtual void Add(TEntity entity)
        {
            using (ISession session = _sessionFactory.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    session.Save(entity);
                    transaction.Commit();
                }
            }
        }

        public virtual void AddRange(IEnumerable<TEntity> entities)
        {
            using (ISession session = _sessionFactory.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    foreach (TEntity entity in entities)
                    {
                        session.Save(entity);
                    }

                    transaction.Commit();
                }
            }
        }

        public virtual TEntity Get(int id)
        {
            TEntity entity;

            using (ISession session = _sessionFactory.OpenSession())
            {
                entity = session.Get<TEntity>(id);
            }

            return entity;
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            var entities = new List<TEntity>();

            using (ISession session = _sessionFactory.OpenSession())
            {
                entities.AddRange(All(session).AsEnumerable());
            }

            return entities;
        }

        public virtual IEnumerable<TEntity> GetAll(Expression<Func<TEntity, object>> sortCondition, SortDirection sortDirection)
        {
            var entities = new List<TEntity>();

            using (ISession session = _sessionFactory.OpenSession())
            {
                entities.AddRange(All(session).SortQueryBy(sortCondition, sortDirection).AsEnumerable());
            }

            return entities;
        }

        public virtual IEnumerable<TEntity> GetRange(Expression<Func<TEntity, bool>> filterCondition)
        {
            var entities = new List<TEntity>();

            using (ISession session = _sessionFactory.OpenSession())
            {
                entities.AddRange(All(session).FilterQueryBy(filterCondition));
            }

            return entities;
        }

        public virtual IEnumerable<TEntity> GetRange(
            Expression<Func<TEntity, bool>> filterCondition,
            Expression<Func<TEntity, object>> sortCondition,
            SortDirection sortDirection)
        {
            var entities = new List<TEntity>();

            using (ISession session = _sessionFactory.OpenSession())
            {
                entities.AddRange(All(session).FilterQueryBy(filterCondition).SortQueryBy(sortCondition, sortDirection));
            }

            return entities;
        }

        public virtual IEnumerable<TEntity> GetRange(
            Expression<Func<TEntity, bool>> filterCondition,
            int pageCapacity,
            int pageNumber)
        {
            var entities = new List<TEntity>();

            using (ISession session = _sessionFactory.OpenSession())
            {
                entities.AddRange(All(session).FilterQueryBy(filterCondition).Skip(pageCapacity * (pageNumber - 1)).Take(pageCapacity));
            }

            return entities;
        }

        public virtual IEnumerable<TEntity> GetRange(
            Expression<Func<TEntity, bool>> filterCondition,
            int pageCapacity,
            int pageNumber,
            Expression<Func<TEntity, object>> sortCondition,
            SortDirection sortDirection)
        {
            var entities = new List<TEntity>();

            using (ISession session = _sessionFactory.OpenSession())
            {
                entities.AddRange(
                    All(session)
                    .FilterQueryBy(filterCondition)
                    .SortQueryBy(sortCondition, sortDirection)
                    .Skip(pageCapacity * (pageNumber - 1))
                    .Take(pageCapacity));
            }

            return entities;
        }

        public virtual void Update(TEntity entity)
        {
            using (ISession session = _sessionFactory.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    session.SaveOrUpdate(entity);
                    transaction.Commit();
                }
            }
        }

        public virtual void UpdateRange(IEnumerable<TEntity> entities)
        {

            using (ISession session = _sessionFactory.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    foreach (TEntity entity in entities)
                    {
                        session.SaveOrUpdate(entity);
                    }

                    transaction.Commit();
                }
            }
        }

        public virtual void Delete(int id)
        {
            using (ISession session = _sessionFactory.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    TEntity entity = Get(id);
                    session.Delete(entity);
                    transaction.Commit();
                }
            }
        }

        public virtual void Delete(TEntity entity)
        {
            using (ISession session = _sessionFactory.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    session.Delete(entity);
                    transaction.Commit();
                }
            }
        }

        public virtual void DeleteRange(IEnumerable<TEntity> entities)
        {
            using (ISession session = _sessionFactory.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    foreach (TEntity entity in entities)
                    {
                        session.Delete(entity);
                    }

                    transaction.Commit();
                }
            }
        }

        public virtual int GetRecordsCount(Expression<Func<TEntity, bool>> filterCondition = null)
        {
            int count;

            using (ISession session = _sessionFactory.OpenSession())
            {
                count = All(session).FilterQueryBy(filterCondition).Count();
            }

            return count;
        }

        public virtual int GetPagesCount(int pageCapacity)
        {
            int count = GetPagesCount(null, pageCapacity);
            return count;
        }

        public virtual int GetPagesCount(Expression<Func<TEntity, bool>> filterCondition, int pageCapacity)
        {
            int recordsCount = GetRecordsCount(filterCondition);
            var pagesCount = (int)Math.Ceiling(recordsCount / (decimal)pageCapacity);
            return pagesCount;
        }

        protected virtual IQueryable<TEntity> All(ISession session)
        {
            IQueryable<TEntity> entities = session.Query<TEntity>();
            return entities;
        }
    }
}