using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TogglTool.Api.Database.Repository;
using TogglTool.Api.Models;

namespace TogglTool.Tests.Api.Database.Repository
{
    [TestFixture]
    public class BaseRepositoryTests
    {
        private Mock<DbSet<Workspace>> _set;
        private Mock<DbContext> _dbContext;
        private IBaseRepository _sut;

        [SetUp]
        public void Setup()
        {
            _set = new Mock<DbSet<Workspace>>(MockBehavior.Strict);
            _dbContext = new Mock<DbContext>(MockBehavior.Strict);
            _dbContext.Setup(x => x.Set<Workspace>()).Returns(_set.Object);
            _sut = new BaseRepository(_dbContext.Object);
        }

        [Test]
        [Ignore("Incomplete")]
        public void AddOrUpdate_Null()
        {
            _sut.AddOrUpdate(default(ICollection<Workspace>));
            // Assert inmemory is empty.
        }

        [Test]
        [Ignore("Incomplete")]
        public void AddOrUpdate_Empty()
        {
            _sut.AddOrUpdate(new List<Workspace>());
            // Assert inmemory is empty.
        }

        [Test]
        [Ignore("Incomplete")]
        public void AddOrUpdate_SingleAdd()
        {
            var list = new List<Workspace>();
            list.Add(new Workspace { id = 1 });
            _sut.AddOrUpdate(list);
            // Assert inmemory contains entity.
        }

        [Test]
        [Ignore("Incomplete")]
        public void AddOrUpdate_Replay()
        {
            var list = new List<Workspace>();
            list.Add(new Workspace { id = 1 });
            _sut.AddOrUpdate(list);
            // Assert inmemory contains entity.
            _sut.AddOrUpdate(list);
            // Assert inmemory is unchanged.
        }

        [Test]
        [Ignore("Incomplete")]
        public void AddOrUpdate_Duplicates()
        {
            var list = new List<Workspace>();
            list.Add(new Workspace { id = 1 });
            list.Add(new Workspace { id = 1 });
            _sut.AddOrUpdate(list);
            // Assert inmemory contains entity only once.
        }

        [Test]
        [Ignore("Incomplete")]
        public void AddOrUpdate_Multiple()
        {
            var list = new List<Workspace>();
            list.Add(new Workspace { id = 1 });
            list.Add(new Workspace { id = 2 });
            _sut.AddOrUpdate(list);
            // Assert inmemory contains all entities.
        }

        [Test]
        [Ignore("Incomplete")]
        public void AddOrUpdate_Update()
        {
            var list1 = new List<Workspace>();
            var list2 = new List<Workspace>();
            list1.Add(new Workspace { id = 1, name = "test 1" });
            list2.Add(new Workspace { id = 1, name = "test 2" });
            _sut.AddOrUpdate(list1);
            // Assert inmemory contains entity.
            _sut.AddOrUpdate(list2);
            // Assert inmemory updated entity.
        }

        [Test]
        [Ignore("Incomplete")]
        public void AddOrUpdate_Mixed()
        {
            var list1 = new List<Workspace>();
            var list2 = new List<Workspace>();
            list1.Add(new Workspace { id = 1, name = "test 1a" });
            list1.Add(new Workspace { id = 2, name = "test 1b" });
            list2.Add(new Workspace { id = 1, name = "test 2a" });
            list2.Add(new Workspace { id = 3, name = "test 2b" });
            _sut.AddOrUpdate(list1);
            // Assert inmemory contains entity.
            _sut.AddOrUpdate(list2);
            // Assert inmemory updated entity.
        }

        //public IEnumerable<T> GetByIds<T>(ICollection<int> idList) where T : TogglEntity;
        //public int SaveChanges();
        //public IEnumerable<T> Where<T>(Expression<Func<T, bool>> predicate, params Expression<Func<T, bool>>[] morePredicates) where T : BaseEntity;
    }
}
