﻿using System;
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

        public IEnumerable<T> GetByIds<T>(ICollection<int> idList)
            where T : TogglEntity
        {
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
            var result = _dbContext.Set<T>().Where(predicate);
            foreach (var x in morePredicates)
                result = result.Where(x);
            return result;
        }
    }
}
