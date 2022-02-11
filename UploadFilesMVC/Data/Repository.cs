using Microsoft.EntityFrameworkCore;
using RickApps.UploadFilesMVC.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RickApps.UploadFilesMVC.Data
{
    /// <summary>
    /// Base class for all CRUD. A few async methods have been added
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext Context;
        protected DbSet<TEntity> _entity;

        public Repository(DbContext context)
        {
            Context = context;
            _entity = Context.Set<TEntity>();
        }

        public TEntity Get(int id)
        {
            // Here we are working with a DbContext, not EFContext. So we don't have DbSets 
            // such as Item or Photo, and we need to use the generic Set() method to access them.
            return _entity.Find(id);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _entity.ToList();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _entity.ToListAsync();
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return _entity.Where(predicate);
        }

        public TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return _entity.SingleOrDefault(predicate);
        }

        public void Add(TEntity entity)
        {
            _entity.Add(entity);
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            _entity.AddRange(entities);
        }

        public void Remove(TEntity entity)
        {
            _entity.Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            _entity.RemoveRange(entities);
        }
    }
}
