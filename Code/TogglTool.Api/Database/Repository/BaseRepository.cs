using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TogglTool.Api.Models;

namespace TogglTool.Api.Database.Repository
{
    public class BaseRepository : IBaseRepository
    {
        public readonly DbContext _dbContext;

        public BaseRepository(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddOrUpdate<T>(ICollection<T> entities)
            where T : TogglEntity
        {
            var set = _dbContext.Set<T>();
            var dbEntities = GetByIds<T>(entities.Select(x => x.id).ToArray()).ToArray();
            var dbIds = dbEntities.Select(x => x.id).ToArray();
            var toBeAdded = entities.Where(x => !dbIds.Contains(x.id));
            var toBeUpdated = entities.Where(x => dbIds.Contains(x.id));
            foreach (var entity in toBeAdded)
                set.Add(entity);
            foreach (var entity in toBeUpdated)
                entity.Update(entities.FirstOrDefault(x => x.id == entity.id));
        }

        public IEnumerable<T> GetByIds<T>(ICollection<int> idList)
            where T : TogglEntity
        {
            return _dbContext.Set<T>().Where(x => idList.Contains(x.id));
        }

        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }

        public IEnumerable<T> Where<T>(Expression<Func<T, bool>> predicate, params Expression<Func<T, bool>>[] morePredicates)
            where T : BaseEntity
        {
            var result = _dbContext.Set<T>().Where(predicate);
            foreach (var x in morePredicates)
                result = result.Where(x);
            return result;
        }
    }
}
