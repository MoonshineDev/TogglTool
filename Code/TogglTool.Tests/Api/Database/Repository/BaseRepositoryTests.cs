using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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

        #region AddOrUpdate
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
            var list = new List<Workspace> {
                new Workspace {id = 1}
            };
            _sut.AddOrUpdate(list);
            var entity = _inmemory.FirstOrDefault(x => x.id == 1);
            Assert.AreEqual(1, _inmemory.Count());
            Assert.NotNull(entity);
        }

        [Test]
        public void AddOrUpdate_Replay()
        {
            var list = new List<Workspace> {
                new Workspace {id = 1}
            };
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
        public void AddOrUpdate_Duplicates()
        {
            var list = new List<Workspace> {
                new Workspace {id = 1},
                new Workspace {id = 1}
            };
            Assert.Catch(() => _sut.AddOrUpdate(list));
            Assert.AreEqual(0, _inmemory.Count());
        }

        [Test]
        public void AddOrUpdate_Multiple()
        {
            var list = new List<Workspace> {
                new Workspace {id = 1},
                new Workspace {id = 2}
            };
            _sut.AddOrUpdate(list);
            var entity1 = _inmemory.FirstOrDefault(x => x.id == 1);
            var entity2 = _inmemory.FirstOrDefault(x => x.id == 2);
            Assert.AreEqual(2, _inmemory.Count());
            Assert.NotNull(entity1);
            Assert.NotNull(entity2);
        }

        [Test]
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
        public void AddOrUpdate_Mixed()
        {
            var list1 = new List<Workspace>();
            var list2 = new List<Workspace>();
            list1.Add(new Workspace { id = 1, name = "test 1a" });
            list1.Add(new Workspace { id = 2, name = "test 1b" });
            list2.Add(new Workspace { id = 1, name = "test 2a" });
            list2.Add(new Workspace { id = 3, name = "test 2b" });

            _sut.AddOrUpdate(list1);
            var entity1A = _inmemory.FirstOrDefault(x => x.id == 1);
            var entity1B = _inmemory.FirstOrDefault(x => x.id == 2);
            Assert.AreEqual(2, _inmemory.Count());
            Assert.NotNull(entity1A);
            Assert.NotNull(entity1B);
            Assert.AreEqual("test 1a", entity1A.name);
            Assert.AreEqual("test 1b", entity1B.name);
            
            _sut.AddOrUpdate(list2);
            var entity2A = _inmemory.FirstOrDefault(x => x.id == 1);
            var entity2B = _inmemory.FirstOrDefault(x => x.id == 2);
            var entity2C = _inmemory.FirstOrDefault(x => x.id == 3);
            Assert.AreEqual(3, _inmemory.Count());
            Assert.NotNull(entity2A);
            Assert.NotNull(entity2B);
            Assert.NotNull(entity2C);
            Assert.AreEqual("test 2a", entity2A.name);
            Assert.AreEqual("test 1b", entity2B.name);
            Assert.AreEqual("test 2b", entity2C.name);
        }

        [Test]
        public void AddOrUpdate_UpdateList()
        {
            var list1 = new List<Workspace>();
            var list2 = new List<Workspace>();
            var client = new Client();
            list1.Add(new Workspace { id = 1, name = "test 1" });
            list2.Add(new Workspace { id = 1, name = "test 1" });
            list2.First().ClientList.Add(client);

            _sut.AddOrUpdate(list1);
            var entity1 = _inmemory.FirstOrDefault(x => x.id == 1);
            Assert.AreEqual(1, _inmemory.Count());
            Assert.NotNull(entity1);
            Assert.AreEqual("test 1", entity1.name);
            Assert.IsEmpty(entity1.ClientList);

            _sut.AddOrUpdate(list2);
            var entity2 = _inmemory.FirstOrDefault(x => x.id == 1);
            Assert.AreEqual(1, _inmemory.Count());
            Assert.NotNull(entity2);
            Assert.AreEqual("test 1", entity2.name);
            Assert.IsNotEmpty(entity1.ClientList);
            Assert.AreEqual(1, entity1.ClientList.Count());
        }
        #endregion

        #region GetByIds
        [Test]
        public void GetByIds_Null()
        {
            Assert.IsEmpty(_sut.GetByIds<Workspace>(null));
        }

        [Test]
        public void GetByIds_Empty()
        {
            Assert.IsEmpty(_sut.GetByIds<Workspace>(Enumerable.Empty<int>()));
        }

        [Test]
        public void GetByIds_NonExisting()
        {
            Assert.IsEmpty(_sut.GetByIds<Workspace>(new[] {1}));
        }

        [Test]
        public void GetByIds_SingleExisting()
        {
            _inmemory.Add(new Workspace {id = 1});
            var list = _sut.GetByIds<Workspace>(new[] {1}).ToArray();
            var entity = list.FirstOrDefault();
            Assert.AreEqual(1, list.Count());
            Assert.IsNotNull(entity);
            Assert.AreEqual(1, entity.id);
        }

        [Test]
        public void GetByIds_Duplicates()
        {
            _inmemory.Add(new Workspace { id = 1 });
            var list = _sut.GetByIds<Workspace>(new[] { 1, 1 }).ToArray();
            var entity = list.FirstOrDefault();
            Assert.AreEqual(1, list.Count());
            Assert.IsNotNull(entity);
            Assert.AreEqual(1, entity.id);
        }

        [Test]
        public void GetByIds_Multiple()
        {
            _inmemory.Add(new Workspace { id = 1 });
            _inmemory.Add(new Workspace { id = 2 });
            var list = _sut.GetByIds<Workspace>(new[] { 1, 2 }).ToArray();
            Assert.AreEqual(2, list.Count());
            Assert.IsTrue(list.Any(x => x.id == 1));
            Assert.IsTrue(list.Any(x => x.id == 2));
        }

        [Test]
        public void GetByIds_MultipleAndMissing()
        {
            _inmemory.Add(new Workspace { id = 1 });
            _inmemory.Add(new Workspace { id = 2 });
            var list = _sut.GetByIds<Workspace>(new[] { 1, 2, 3 }).ToArray();
            Assert.AreEqual(2, list.Count());
            Assert.IsTrue(list.Any(x => x.id == 1));
            Assert.IsTrue(list.Any(x => x.id == 2));
            Assert.IsFalse(list.Any(x => x.id == 3));
        }
        #endregion

        #region SaveChanges
        [Test]
        public void SaveChanges()
        {
            _dbContext.Setup(x => x.SaveChanges()).Returns(0);
            Assert.AreEqual(0, _sut.SaveChanges());
            _dbContext.Setup(x => x.SaveChanges()).Returns(1);
            Assert.AreEqual(1, _sut.SaveChanges());
            _dbContext.Verify(x => x.SaveChanges(), Times.Exactly(2));
        }
        #endregion

        #region Where
        [Test]
        public void Where_Null()
        {
            _inmemory.Add(new Workspace { id = 1 });
            _inmemory.Add(new Workspace { id = 2 });
            _inmemory.Add(new Workspace { id = 3 });
            Assert.IsEmpty(_sut.Where<Workspace>(null));
        }

        [Test]
        public void Where_NullAndAll()
        {
            _inmemory.Add(new Workspace { id = 1 });
            _inmemory.Add(new Workspace { id = 2 });
            _inmemory.Add(new Workspace { id = 3 });
            var list = _sut.Where<Workspace>(null, x => true).ToArray();
            Assert.AreEqual(3, list.Count());
            Assert.IsTrue(list.Any(x => x.id == 1));
            Assert.IsTrue(list.Any(x => x.id == 2));
            Assert.IsTrue(list.Any(x => x.id == 3));
        }

        [Test]
        public void Where_NullAndNull()
        {
            _inmemory.Add(new Workspace { id = 1 });
            _inmemory.Add(new Workspace { id = 2 });
            _inmemory.Add(new Workspace { id = 3 });
            Assert.IsEmpty(_sut.Where<Workspace>(null, null).ToArray());
        }

        [Test]
        public void Where_All()
        {
            _inmemory.Add(new Workspace { id = 1 });
            _inmemory.Add(new Workspace { id = 2 });
            _inmemory.Add(new Workspace { id = 3 });
            var list = _sut.Where<Workspace>(x => true).ToArray();
            Assert.AreEqual(3, list.Count());
            Assert.IsTrue(list.Any(x => x.id == 1));
            Assert.IsTrue(list.Any(x => x.id == 2));
            Assert.IsTrue(list.Any(x => x.id == 3));
        }

        [Test]
        public void Where_None()
        {
            _inmemory.Add(new Workspace { id = 1 });
            _inmemory.Add(new Workspace { id = 2 });
            _inmemory.Add(new Workspace { id = 3 });
            Assert.IsEmpty(_sut.Where<Workspace>(x => false).ToArray());
        }

        [Test]
        public void Where_Some()
        {
            _inmemory.Add(new Workspace { id = 1 });
            _inmemory.Add(new Workspace { id = 2 });
            _inmemory.Add(new Workspace { id = 3 });
            var list = _sut.Where<Workspace>(x => x.id >= 2).ToArray();
            Assert.AreEqual(2, list.Count());
            Assert.IsFalse(list.Any(x => x.id == 1));
            Assert.IsTrue(list.Any(x => x.id == 2));
            Assert.IsTrue(list.Any(x => x.id == 3));
        }

        [Test]
        public void Where_SomeAndNull()
        {
            _inmemory.Add(new Workspace { id = 1 });
            _inmemory.Add(new Workspace { id = 2 });
            _inmemory.Add(new Workspace { id = 3 });
            var list = _sut.Where<Workspace>(x => x.id >= 2, null).ToArray();
            Assert.AreEqual(2, list.Count());
            Assert.IsFalse(list.Any(x => x.id == 1));
            Assert.IsTrue(list.Any(x => x.id == 2));
            Assert.IsTrue(list.Any(x => x.id == 3));
        }

        [Test]
        public void Where_SomeAndAll()
        {
            _inmemory.Add(new Workspace { id = 1 });
            _inmemory.Add(new Workspace { id = 2 });
            _inmemory.Add(new Workspace { id = 3 });
            var list = _sut.Where<Workspace>(x => x.id >= 2, x => true).ToArray();
            Assert.AreEqual(2, list.Count());
            Assert.IsFalse(list.Any(x => x.id == 1));
            Assert.IsTrue(list.Any(x => x.id == 2));
            Assert.IsTrue(list.Any(x => x.id == 3));
        }

        [Test]
        public void Where_SomeAndNone()
        {
            _inmemory.Add(new Workspace { id = 1 });
            _inmemory.Add(new Workspace { id = 2 });
            _inmemory.Add(new Workspace { id = 3 });
            Assert.IsEmpty(_sut.Where<Workspace>(x => x.id >= 2, x => false).ToArray());
        }

        [Test]
        public void Where_SomeAndSome()
        {
            _inmemory.Add(new Workspace { id = 1 });
            _inmemory.Add(new Workspace { id = 2 });
            _inmemory.Add(new Workspace { id = 3 });
            var list = _sut.Where<Workspace>(x => x.id >= 2, x => x.id <= 2).ToArray();
            var entity = list.FirstOrDefault();
            Assert.AreEqual(1, list.Count());
            Assert.IsNotNull(entity);
            Assert.AreEqual(2, entity.id);
        }
        #endregion
    }
}
