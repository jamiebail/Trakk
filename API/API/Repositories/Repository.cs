using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using API.Models;

namespace API.Repositories
{
    public class Repository<TEntity>: IRepository<TEntity> where TEntity : class
    {
        public ApplicationDbContext Data;
        public Repository()
        {
            Data = new ApplicationDbContext();
        }
        public List<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate)
        {
            return Data.Set<TEntity>().Where(predicate).ToList();
        }
        public List<TEntity> GetAll()
        {
            return Data.Set<TEntity>().ToList();
        }
        public TEntity Add(TEntity entity)
        {
            Data.Set<TEntity>().Add(entity);
            Save();
            return entity;
        }
        public void Remove(TEntity entity)
        {
            Data.Set<TEntity>().Remove(entity);
            Save();
        }
        public void Update(TEntity entity)
        {
            Data.Set<TEntity>().AddOrUpdate(entity);
            Save();
        }
        public void Save()
        {
            Data.SaveChanges();
        }
    }
}