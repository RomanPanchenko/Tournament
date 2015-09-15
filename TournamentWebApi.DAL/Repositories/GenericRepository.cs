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
            ISession session = _sessionFactory.OpenSession();
            using (ITransaction transaction = session.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                session.Save(entity);
                transaction.Commit();
                session.Dispose();
            }
        }

        public virtual void AddRange(IEnumerable<TEntity> entities)
        {
            ISession session = _sessionFactory.OpenSession();
            using (ITransaction transaction = session.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                foreach (TEntity entity in entities)
                {
                    session.Save(entity);
                }

                transaction.Commit();
                session.Dispose();
            }
        }

        public virtual TEntity Get(int id)
        {
            ISession session = _sessionFactory.OpenSession();
            var entity = session.Get<TEntity>(id);
            session.Dispose();
            return entity;
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return All();
        }

        public IEnumerable<TEntity> GetAll(Expression<Func<TEntity, object>> sortCondition, SortDirection sortDirection)
        {
            IQueryable<TEntity> result = All().SortQueryBy(sortCondition, sortDirection);
            return result;
        }

        public virtual IEnumerable<TEntity> GetRange(Expression<Func<TEntity, bool>> filterCondition)
        {
            IQueryable<TEntity> result = All().FilterQueryBy(filterCondition);
            return result;
        }

        public IEnumerable<TEntity> GetRange(
            Expression<Func<TEntity, bool>> filterCondition,
            Expression<Func<TEntity, object>> sortCondition,
            SortDirection sortDirection)
        {
            IQueryable<TEntity> result = All()
                .FilterQueryBy(filterCondition)
                .SortQueryBy(sortCondition, sortDirection);

            return result;
        }

        public virtual IEnumerable<TEntity> GetRange(
            Expression<Func<TEntity, bool>> filterCondition,
            int pageCapacity,
            int pageNumber)
        {
            IQueryable<TEntity> result = All()
                .FilterQueryBy(filterCondition)
                .Skip(pageCapacity * (pageNumber - 1))
                .Take(pageCapacity);

            return result;
        }

        public virtual IEnumerable<TEntity> GetRange(
            Expression<Func<TEntity, bool>> filterCondition,
            int pageCapacity,
            int pageNumber,
            Expression<Func<TEntity, object>> sortCondition,
            SortDirection sortDirection)
        {
            IQueryable<TEntity> result = All()
                .FilterQueryBy(filterCondition)
                .SortQueryBy(sortCondition, sortDirection)
                .Skip(pageCapacity * (pageNumber - 1))
                .Take(pageCapacity);

            return result;
        }

        public virtual void Update(TEntity entity)
        {
            ISession session = _sessionFactory.OpenSession();
            using (ITransaction transaction = session.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                session.SaveOrUpdate(entity);
                transaction.Commit();
                session.Dispose();
            }
        }

        public virtual void UpdateRange(IEnumerable<TEntity> entities)
        {
            ISession session = _sessionFactory.OpenSession();
            using (ITransaction transaction = session.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                foreach (TEntity entity in entities)
                {
                    session.SaveOrUpdate(entity);
                }

                transaction.Commit();
                session.Dispose();
            }
        }

        public virtual void Delete(int id)
        {
            ISession session = _sessionFactory.OpenSession();
            using (ITransaction transaction = session.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                TEntity entity = Get(id);
                session.Delete(entity);
                transaction.Commit();
                session.Dispose();
            }
        }

        public virtual void Delete(TEntity entity)
        {
            ISession session = _sessionFactory.OpenSession();
            using (ITransaction transaction = session.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                session.Delete(entity);
                transaction.Commit();
                session.Dispose();
            }
        }

        public virtual void DeleteRange(IEnumerable<TEntity> entities)
        {
            ISession session = _sessionFactory.OpenSession();
            using (ITransaction transaction = session.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                foreach (TEntity entity in entities)
                {
                    session.Delete(entity);
                }

                transaction.Commit();
                session.Dispose();
            }
        }

        public virtual int GetRecordsCount(Expression<Func<TEntity, bool>> filterCondition = null)
        {
            int count = All().FilterQueryBy(filterCondition).Count();
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

        protected virtual IQueryable<TEntity> All()
        {
            ISession session = _sessionFactory.OpenSession();
            IQueryable<TEntity> entities = session.Query<TEntity>();
            session.Dispose();
            return entities;
        }
    }
}