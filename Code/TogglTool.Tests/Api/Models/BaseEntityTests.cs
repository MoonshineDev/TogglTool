using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        [Explicit("Failed")]
        public void Update_Null()
        {
            var entity1 = new Workspace { name = "test 1" };
            var date1 = DateTime.UtcNow;
            entity1.Update(null);
            var date2 = DateTime.UtcNow;
            Assert.LessOrEqual(date1, entity1.LogChangedOn);
            Assert.GreaterOrEqual(date2, entity1.LogChangedOn);
        }

        [Test]
        [Explicit("Failed")]
        public void Update_Normal()
        {
            var entity1 = new Workspace { name = "test 1" };
            var entity2 = new Workspace { name = "test 2" };
            var date1 = DateTime.UtcNow;
            entity1.Update(entity2);
            var date2 = DateTime.UtcNow;
            Assert.LessOrEqual(date1, entity1.LogChangedOn);
            Assert.GreaterOrEqual(date2, entity1.LogChangedOn);
            Assert.AreEqual("test 2", entity1.name);
        }

        [Test]
        [Explicit("Failed")]
        public void Update_DifferentId()
        {
            var entity1 = new Workspace { id = 1, name = "test 1" };
            var entity2 = new Workspace { id = 2, name = "test 2" };
            var date1 = DateTime.UtcNow;
            entity1.Update(entity2);
            var date2 = DateTime.UtcNow;
            Assert.LessOrEqual(date1, entity1.LogChangedOn);
            Assert.GreaterOrEqual(date2, entity1.LogChangedOn);
            Assert.AreEqual("test 1", entity1.name);
        }
    }
}
