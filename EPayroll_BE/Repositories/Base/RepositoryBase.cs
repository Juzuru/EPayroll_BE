using EPayroll_BE.Data;
using EPayroll_BE.Models.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace EPayroll_BE.Repositories.Base
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : ModelBase
    {
        protected readonly EPayrollContext context;
        protected readonly DbSet<T> dbSet;

        public RepositoryBase(EPayrollContext context)
        {
            this.context = context;
            dbSet = context.Set<T>();
        }

        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public void Add(IEnumerable<T> list)
        {
            dbSet.AddRange(list);
        }

        public void Delete(T entity)
        {
            dbSet.Remove(entity);
        }

        public void Delete(Expression<Func<T, bool>> where)
        {
            dbSet.Remove(Get(where).First());
        }
        public IList<T> GetAll()
        {
            return dbSet.ToList();
        }

        public T GetById(Guid id)
        {
            return dbSet.Find(id);
        }

        public IList<T> Get(Expression<Func<T, bool>> where)
        {
            return dbSet.Where(where).ToList();
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }

        public void Update(T entity)
        {
            dbSet.Update(entity);
        }

        public int Count()
        {
            return dbSet.Count();
        }

        public int Count(Expression<Func<T, bool>> where)
        {
            return dbSet.Where(where).Count();
        }
    }

    public interface IRepositoryBase<T> where T : class
    {
        void Add(T entity);
        void Add(IEnumerable<T> list);
        IList<T> GetAll();
        T GetById(Guid id);
        IList<T> Get(Expression<Func<T, bool>> where);
        void Update(T entity);
        void Delete(T entity);
        void Delete(Expression<Func<T, bool>> where);
        int Count();
        int Count(Expression<Func<T, bool>> where);
        void SaveChanges();
    }
}
