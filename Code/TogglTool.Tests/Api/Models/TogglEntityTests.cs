using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using TogglTool.Api.Models;

namespace TogglTool.Tests.Api.Models
{
    [TestFixture]
    public class TogglEntityTests
    {
        [Test]
        public void Update_Null()
        {
            var entity1 = new TogglTestEntity { id = 1, Name = "test 1" };
            Assert.IsFalse(entity1.Update(null));
            Assert.AreEqual("test 1", entity1.Name);
            Assert.AreEqual(null, entity1.LogChangedOn);
        }

        [Test]
        public void Update_Normal()
        {
            var entity1 = new TogglTestEntity { id = 1, Name = "test 1" };
            var entity2 = new TogglTestEntity { id = 1, Name = "test 2" };
            var date1 = DateTime.UtcNow;
            Assert.IsTrue(entity1.Update(entity2));
            var date2 = DateTime.UtcNow;
            Assert.AreEqual("test 2", entity1.Name);
            Assert.LessOrEqual(date1, entity1.LogChangedOn);
            Assert.GreaterOrEqual(date2, entity1.LogChangedOn);
        }

        [Test]
        public void Update_Unchanged()
        {
            var entity1 = new TogglTestEntity { id = 1, Name = "test 1" };
            var entity2 = new TogglTestEntity { id = 1, Name = "test 1" };
            Assert.IsFalse(entity1.Update(entity2));
            Assert.AreEqual("test 1", entity1.Name);
            Assert.AreEqual(null, entity1.LogChangedOn);
        }

        [Test]
        public void Update_DifferentId()
        {
            var entity1 = new TogglTestEntity { id = 1, Name = "test 1" };
            var entity2 = new TogglTestEntity { id = 2, Name = "test 2" };
            Assert.IsFalse(entity1.Update(entity2));
            Assert.AreEqual("test 1", entity1.Name);
            Assert.AreEqual(null, entity1.LogChangedOn);
        }

        private class TogglTestEntity : TogglEntity
        {
            public string Name { get; set; }
        }
    }
}
