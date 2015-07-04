using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TogglTool.Api.Models;

namespace TogglTool.Api.Database.Repository
{
    public interface IBaseRepository
    {
        IEnumerable<T> GetByIds<T>(ICollection<int> idList) where T : TogglEntity;
        IEnumerable<T> Where<T>(Expression<Func<T, bool>> predicate, params Expression<Func<T, bool>>[] morePredicates) where T : BaseEntity;
    }
}
