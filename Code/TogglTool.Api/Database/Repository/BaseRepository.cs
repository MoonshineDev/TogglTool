using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using TogglTool.Api.Models;

namespace TogglTool.Api.Database.Repository
{
    public class BaseRepository : IBaseRepository
    {
        private readonly DbContext _dbContext;

        public BaseRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddOrUpdate<T>(IEnumerable<T> entities)
            where T : TogglEntity
        {
            if (entities == null)
                return;
            var set = _dbContext.Set<T>();
            var ids = entities.Select(x => x.id).ToArray();
            if (ids.Count() != ids.Distinct().Count())
                throw new ArgumentException("No duplicates allowed");
            var dbEntities = GetByIds<T>(ids).ToArray();
            foreach (var entity in entities)
            {
                var dbEntity = dbEntities.FirstOrDefault(x => x.id == entity.id);
                if (dbEntity == null)
                    set.Add(entity);
                else
                    dbEntity.Update(entity);
            }
        }

        public IEnumerable<T> GetByIds<T>(IEnumerable<int> idList)
            where T : TogglEntity
        {
            if (idList == null)
                return Enumerable.Empty<T>();
            var ids = idList.Distinct().ToArray();
            return _dbContext.Set<T>().Where(x => ids.Contains(x.id));
        }

        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }

        public IEnumerable<T> Where<T>(Expression<Func<T, bool>> predicate, params Expression<Func<T, bool>>[] morePredicates)
            where T : BaseEntity
        {
            if (predicate == null && (morePredicates == null || morePredicates.All(x => x == null)))
                return Enumerable.Empty<T>();
            var result = _dbContext.Set<T>() as IQueryable<T>;
            if (predicate != null)
                result = result.Where(predicate);
            if (morePredicates == null)
                return result;
            foreach (var x in morePredicates.Where(x => x != null))
                result = result.Where(x);
            return result;
        }
    }
}
