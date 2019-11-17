using EPayroll_BE.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace EPayroll_BE.Repositories
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
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
        public IQueryable<T> GetAll()
        {
            return dbSet;
        }

        public T GetById(string id)
        {
            return dbSet.Find(id);
        }

        public IQueryable<T> Get(Expression<Func<T, bool>> where)
        {
            return dbSet.Where(where);
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }

        public void Update(T entity)
        {
            dbSet.Update(entity);
        }
    }

    public interface IRepositoryBase<T> where T : class
    {
        void Add(T entity);
        void Add(IEnumerable<T> list);
        IQueryable<T> GetAll();
        T GetById(string id);
        IQueryable<T> Get(Expression<Func<T, bool>> where);
        void Update(T entity);
        void Delete(T entity);
        void Delete(Expression<Func<T, bool>> where);
        void SaveChanges();
    }
}
