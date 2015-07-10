using System;
using NUnit.Framework;
using TogglTool.Api.Models;

namespace TogglTool.Tests.Api.Models
{
    [TestFixture]
    public class WebApiEntityTests
    {
        [Test]
        public void Update_Null()
        {
            var entity1 = new WebApiTestEntity { Name = "test 1" };
            var date1 = DateTime.UtcNow;
            Assert.IsFalse(entity1.Update(null));
            var date2 = DateTime.UtcNow;
            Assert.AreEqual("test 1", entity1.Name);
            Assert.LessOrEqual(date1, entity1.LastRequestedOn);
            Assert.GreaterOrEqual(date2, entity1.LastRequestedOn);
            Assert.AreEqual(null, entity1.LastChangedOn);
        }

        [Test]
        public void Update_Normal()
        {
            var entity1 = new WebApiTestEntity { Name = "test 1" };
            var entity2 = new WebApiTestEntity { Name = "test 2" };
            var date1 = DateTime.UtcNow;
            Assert.IsTrue(entity1.Update(entity2));
            var date2 = DateTime.UtcNow;
            Assert.AreEqual("test 2", entity1.Name);
            Assert.LessOrEqual(date1, entity1.LastRequestedOn);
            Assert.GreaterOrEqual(date2, entity1.LastRequestedOn);
            Assert.LessOrEqual(date1, entity1.LastChangedOn);
            Assert.GreaterOrEqual(date2, entity1.LastChangedOn);
        }

        [Test]
        public void Update_Unchanged()
        {
            var entity1 = new WebApiTestEntity { Name = "test 1" };
            var entity2 = new WebApiTestEntity { Name = "test 1" };
            var date1 = DateTime.UtcNow;
            Assert.IsFalse(entity1.Update(entity2));
            var date2 = DateTime.UtcNow;
            Assert.AreEqual("test 1", entity1.Name);
            Assert.LessOrEqual(date1, entity1.LastRequestedOn);
            Assert.GreaterOrEqual(date2, entity1.LastRequestedOn);
            Assert.AreEqual(null, entity1.LastChangedOn);
        }

        private class WebApiTestEntity : WebApiEntity
        {
            public string Name { get; set; }
        }
    }
}
