using NUnit.Framework;
using System;
using TogglTool.Api.Models;

namespace TogglTool.Tests.Api.Models
{
    [TestFixture]
    public class BaseEntityTests
    {
        [Test]
        public void Ctor_LogCreatedOn()
        {
            var date1 = DateTime.UtcNow;
            var entity = new Workspace();
            var date2 = DateTime.UtcNow;
            Assert.LessOrEqual(date1, entity.LogCreatedOn);
            Assert.GreaterOrEqual(date2, entity.LogCreatedOn);
        }

        [Test]
        public void Update_Null()
        {
            var entity1 = new Workspace { name = "test 1" };
            Assert.IsFalse(entity1.Update(null));
            Assert.AreEqual(null, entity1.LogChangedOn);
        }

        [Test]
        public void Update_Normal()
        {
            var entity1 = new Workspace { name = "test 1" };
            var entity2 = new Workspace { name = "test 2" };
            var date1 = DateTime.UtcNow;
            Assert.IsTrue(entity1.Update(entity2));
            var date2 = DateTime.UtcNow;
            Assert.IsNotNull(entity1.LogChangedOn);
            Assert.LessOrEqual(date1, entity1.LogChangedOn);
            Assert.GreaterOrEqual(date2, entity1.LogChangedOn);
            Assert.AreEqual("test 2", entity1.name);
        }

        [Test]
        public void Update_Unchanged()
        {
            var entity1 = new Workspace { name = "test 1" };
            var entity2 = new Workspace { name = "test 1" };
            Assert.IsFalse(entity1.Update(entity2));
            Assert.AreEqual("test 1", entity1.name);
            Assert.AreEqual(null, entity1.LogChangedOn);
        }

        [Test]
        public void Update_DifferentId()
        {
            var entity1 = new Workspace { id = 1, name = "test 1" };
            var entity2 = new Workspace { id = 2, name = "test 2" };
            Assert.IsFalse(entity1.Update(entity2));
            Assert.AreEqual("test 1", entity1.name);
            Assert.AreEqual(null, entity1.LogChangedOn);
        }
    }
}
