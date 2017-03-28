using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using API.Models;

namespace API.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        List<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate);
        List<TEntity> GetAll();
        TEntity Add(TEntity entity);
        void Remove(TEntity entity);
        void Update(TEntity entity);
        void Save();
    }
}
