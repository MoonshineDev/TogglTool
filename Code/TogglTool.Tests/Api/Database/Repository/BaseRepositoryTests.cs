using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
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

        private ICollection<Workspace> _inmemory;

        [SetUp]
        public void Setup()
        {
            _inmemory = new List<Workspace>();
            _set = _inmemory.ToDbSet(MockBehavior.Strict);
            _dbContext = new Mock<DbContext>(MockBehavior.Strict);
            _dbContext.Setup(x => x.Set<Workspace>()).Returns(_set.Object);
            _sut = new BaseRepository(_dbContext.Object);
        }

        [Test]
        public void AddOrUpdate_Null()
        {
            _sut.AddOrUpdate(default(ICollection<Workspace>));
            Assert.IsEmpty(_inmemory);
        }

        [Test]
        public void AddOrUpdate_Empty()
        {
            _sut.AddOrUpdate(new List<Workspace>());
            Assert.IsEmpty(_inmemory);
        }

        [Test]
        public void AddOrUpdate_SingleAdd()
        {
            var list = new List<Workspace>();
            list.Add(new Workspace { id = 1 });
            _sut.AddOrUpdate(list);
            var entity = _inmemory.FirstOrDefault(x => x.id == 1);
            Assert.AreEqual(1, _inmemory.Count());
            Assert.NotNull(entity);
        }

        [Test]
        public void AddOrUpdate_Replay()
        {
            var list = new List<Workspace>();
            list.Add(new Workspace { id = 1 });

            _sut.AddOrUpdate(list);
            var entity1 = _inmemory.FirstOrDefault(x => x.id == 1);
            Assert.AreEqual(1, _inmemory.Count());
            Assert.NotNull(entity1);
            
            _sut.AddOrUpdate(list);
            var entity2 = _inmemory.FirstOrDefault(x => x.id == 1);
            Assert.AreEqual(1, _inmemory.Count());
            Assert.NotNull(entity2);
            Assert.AreEqual(entity1.LogChangedBy, entity2.LogChangedBy);
            Assert.AreEqual(entity1.LogChangedOn, entity2.LogChangedOn);
            Assert.AreEqual(entity1.LogCreatedBy, entity2.LogCreatedBy);
            Assert.AreEqual(entity1.LogCreatedOn, entity2.LogCreatedOn);
        }

        [Test]
        [Explicit("Failed")]
        public void AddOrUpdate_Duplicates()
        {
            var list = new List<Workspace>();
            list.Add(new Workspace { id = 1 });
            list.Add(new Workspace { id = 1 });
            _sut.AddOrUpdate(list);
            var entity = _inmemory.FirstOrDefault(x => x.id == 1);
            Assert.AreEqual(1, _inmemory.Count());
            Assert.NotNull(entity);
        }

        [Test]
        public void AddOrUpdate_Multiple()
        {
            var list = new List<Workspace>();
            list.Add(new Workspace { id = 1 });
            list.Add(new Workspace { id = 2 });
            _sut.AddOrUpdate(list);
            var entity1 = _inmemory.FirstOrDefault(x => x.id == 1);
            var entity2 = _inmemory.FirstOrDefault(x => x.id == 2);
            Assert.AreEqual(2, _inmemory.Count());
            Assert.NotNull(entity1);
            Assert.NotNull(entity2);
        }

        [Test]
        [Explicit("Failed")]
        public void AddOrUpdate_Update()
        {
            var list1 = new List<Workspace>();
            var list2 = new List<Workspace>();
            list1.Add(new Workspace { id = 1, name = "test 1" });
            list2.Add(new Workspace { id = 1, name = "test 2" });
            
            _sut.AddOrUpdate(list1);
            var entity1 = _inmemory.FirstOrDefault(x => x.id == 1);
            Assert.AreEqual(1, _inmemory.Count());
            Assert.NotNull(entity1);
            Assert.AreEqual("test 1", entity1.name);

            _sut.AddOrUpdate(list2);
            var entity2 = _inmemory.FirstOrDefault(x => x.id == 1);
            Assert.AreEqual(1, _inmemory.Count());
            Assert.NotNull(entity2);
            Assert.AreEqual("test 2", entity2.name);
        }

        [Test]
        [Explicit("Failed")]
        public void AddOrUpdate_Mixed()
        {
            var list1 = new List<Workspace>();
            var list2 = new List<Workspace>();
            list1.Add(new Workspace { id = 1, name = "test 1a" });
            list1.Add(new Workspace { id = 2, name = "test 1b" });
            list2.Add(new Workspace { id = 1, name = "test 2a" });
            list2.Add(new Workspace { id = 3, name = "test 2b" });

            _sut.AddOrUpdate(list1);
            var entity1a = _inmemory.FirstOrDefault(x => x.id == 1);
            var entity1b = _inmemory.FirstOrDefault(x => x.id == 2);
            Assert.AreEqual(2, _inmemory.Count());
            Assert.NotNull(entity1a);
            Assert.NotNull(entity1b);
            Assert.AreEqual("test 1a", entity1a.name);
            Assert.AreEqual("test 1b", entity1b.name);
            
            _sut.AddOrUpdate(list2);
            var entity2a = _inmemory.FirstOrDefault(x => x.id == 1);
            var entity2b = _inmemory.FirstOrDefault(x => x.id == 2);
            var entity2c = _inmemory.FirstOrDefault(x => x.id == 3);
            Assert.AreEqual(3, _inmemory.Count());
            Assert.NotNull(entity2a);
            Assert.NotNull(entity2b);
            Assert.NotNull(entity2c);
            Assert.AreEqual("test 2a", entity2a.name);
            Assert.AreEqual("test 1b", entity2b.name);
            Assert.AreEqual("test 2b", entity2c.name);
        }

        //public IEnumerable<T> GetByIds<T>(ICollection<int> idList) where T : TogglEntity;
        //public int SaveChanges();
        //public IEnumerable<T> Where<T>(Expression<Func<T, bool>> predicate, params Expression<Func<T, bool>>[] morePredicates) where T : BaseEntity;
    }
}
