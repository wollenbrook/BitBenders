using System.Linq.Expressions;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace BitBracket.DAL.Abstract
{
    public interface IRepository<TEntity> where TEntity : class, new()
    {
        TEntity FindById(int id);
        bool Exists(int id);
        IQueryable<TEntity> GetAll();
        IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate);
        IQueryable<TEntity> GetAll(params Expression<Func<TEntity, object>>[] includes);
        TEntity AddOrUpdate(TEntity entity);
        void Delete(TEntity entity);
        void DeleteById(int id);
    }
}
