using Moq;
using System.Data.Entity;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace System.Collections.Generic
{
    public static class CollectionExtensions
    {
        public static Mock<DbSet<T>> ToDbSet<T>(this ICollection<T> @this, MockBehavior behavior = MockBehavior.Default)
            where T : class
        {
            var queryable = @this.AsQueryable();
            var mock = new Mock<DbSet<T>>(behavior);
            mock.As<IQueryable<T>>().Setup(x => x.Provider).Returns(queryable.Provider);
            mock.As<IQueryable<T>>().Setup(x => x.Expression).Returns(queryable.Expression);
            mock.As<IQueryable<T>>().Setup(x => x.ElementType).Returns(queryable.ElementType);
            mock.As<IQueryable<T>>().Setup(x => x.GetEnumerator()).Returns(queryable.GetEnumerator());
            mock.Setup(x => x.Add(It.IsAny<T>()))
                .Callback((T entity) => @this.Add(entity))
                .Returns((T entity) => entity);
            return mock;
        }
    }
}
