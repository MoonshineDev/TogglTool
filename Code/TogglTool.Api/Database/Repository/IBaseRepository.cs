using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using TogglTool.Api.Models;

namespace TogglTool.Api.Database.Repository
{
    public interface IBaseRepository
    {
        void AddOrUpdate<T>(IEnumerable<T> entities) where T : TogglEntity;
        IEnumerable<T> GetByIds<T>(IEnumerable<int> idList) where T : TogglEntity;
        int SaveChanges();
        IEnumerable<T> Where<T>(Expression<Func<T, bool>> predicate, params Expression<Func<T, bool>>[] morePredicates) where T : BaseEntity;
    }
}
