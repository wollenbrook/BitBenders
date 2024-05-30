﻿using BitBracket.DAL.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq;
using System;
using System.Collections.Generic;
namespace BitBracket.DAL.Concrete
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, new()
    {
        private readonly DbContext _context;
        protected readonly DbSet<TEntity> _dbSet;
        public Repository(DbContext ctx)
        {
            _context = ctx;
            _dbSet = _context.Set<TEntity>();
        }
        public virtual TEntity FindById(int id)
        {
            TEntity entity = _dbSet.Find(id);
            return entity;  // null if not found
        }
        public virtual bool Exists(int id)
        {
            return FindById(id) != null;
        }
        public virtual IQueryable<TEntity> GetAll()
        {
            return _dbSet;
        }
        public IQueryable<TEntity> GetAll(params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> dbs = _dbSet;
            foreach (var item in includes)
            {
                dbs = dbs.Include(item);
            }
            return dbs;
        }
        public virtual IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate)
        {
            return _dbSet.Where(predicate);
        }
        public IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> dbs = _dbSet;
            foreach (var item in includes)
            {
                dbs = dbs.Include(item);
            }
            return dbs.Where(predicate);
        }
        public virtual TEntity AddOrUpdate(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("Entity must not be null to add or update");
            }
            _context.Update(entity);
            _context.SaveChanges();
            return entity;
        }
        public virtual void Delete(TEntity entity)
        {
            if (entity == null)
            {
                throw new Exception("Entity to delete was not found or was null");
            }
            else
            {
                _dbSet.Remove(entity);
                _context.SaveChanges();
            }
            return;
        }
        public virtual void DeleteById(int id)
        {
            Delete(FindById(id));
            return;
        }
    }
}