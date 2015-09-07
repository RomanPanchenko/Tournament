using NHibernate;
using Ninject;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using TournamentWebApi.Core.Enums;
using TournamentWebApi.DAL.Extensions;
using TournamentWebApi.DAL.Interfaces;
using TournamentWebApi.DAL.Wrappers;

namespace TournamentWebApi.DAL.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly ISession _session;

        public GenericRepository(ISession session)
        {
            _session = session;
        }

        [Inject]
        public INhSession<TEntity> NhSession { get; set; }

        public virtual void Add(TEntity entity)
        {
            using (ITransaction transaction = _session.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                _session.Save(entity);
                transaction.Commit();
            }
        }

        public virtual void AddRange(IEnumerable<TEntity> entities)
        {
            using (ITransaction transaction = _session.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                foreach (TEntity entity in entities)
                {
                    _session.Save(entity);
                }

                transaction.Commit();
            }
        }

        public virtual TEntity Get(int id)
        {
            return _session.Get<TEntity>(id);
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return All();
        }

        public IEnumerable<TEntity> GetAll(Expression<Func<TEntity, object>> sortCondition, SortDirection sortDirection)
        {
            IQueryable<TEntity> result = All().SortBy(sortCondition, sortDirection);
            return result;
        }

        public virtual IEnumerable<TEntity> GetRange(Expression<Func<TEntity, bool>> filterCondition)
        {
            IQueryable<TEntity> result = All().FilterBy(filterCondition);
            return result;
        }

        public IEnumerable<TEntity> GetRange(
            Expression<Func<TEntity, bool>> filterCondition,
            Expression<Func<TEntity, object>> sortCondition,
            SortDirection sortDirection)
        {
            IQueryable<TEntity> result = All()
                .FilterBy(filterCondition)
                .SortBy(sortCondition, sortDirection);

            return result;
        }

        public virtual IEnumerable<TEntity> GetRange(
            Expression<Func<TEntity, bool>> filterCondition,
            int pageCapacity,
            int pageNumber)
        {
            IQueryable<TEntity> result = All()
                .FilterBy(filterCondition)
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
                .FilterBy(filterCondition)
                .SortBy(sortCondition, sortDirection)
                .Skip(pageCapacity * (pageNumber - 1))
                .Take(pageCapacity);

            return result;
        }

        public virtual void Update(TEntity entity)
        {
            using (ITransaction transaction = _session.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                _session.SaveOrUpdate(entity);
                transaction.Commit();
            }
        }

        public virtual void UpdateRange(IEnumerable<TEntity> entities)
        {
            using (ITransaction transaction = _session.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                foreach (TEntity entity in entities)
                {
                    _session.SaveOrUpdate(entity);
                }

                transaction.Commit();
            }
        }

        public virtual void Delete(int id)
        {
            using (ITransaction transaction = _session.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                TEntity entity = Get(id);
                _session.Delete(entity);
                transaction.Commit();
            }
        }

        public virtual void Delete(TEntity entity)
        {
            using (ITransaction transaction = _session.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                _session.Delete(entity);
                transaction.Commit();
            }
        }

        public virtual void DeleteRange(IEnumerable<TEntity> entities)
        {
            using (ITransaction transaction = _session.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                foreach (TEntity entity in entities)
                {
                    _session.Delete(entity);
                }

                transaction.Commit();
            }
        }

        public virtual int GetRecordsCount(Expression<Func<TEntity, bool>> filterCondition = null)
        {
            int count = All().FilterBy(filterCondition).Count();
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

        private IQueryable<TEntity> All()
        {
            return NhSession.Query();
        }
    }
}