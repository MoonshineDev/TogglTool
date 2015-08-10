using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var entity = new BaseTestEntity();
            var date2 = DateTime.UtcNow;
            Assert.LessOrEqual(date1, entity.CreatedOn);
            Assert.GreaterOrEqual(date2, entity.CreatedOn);
        }

        [Test]
        public void Update_Null()
        {
            var entity1 = new BaseTestEntity { Name = "test 1" };
            Assert.IsFalse(entity1.Update(null));
            Assert.AreEqual(null, entity1.LogChangedOn);
        }

        [Test]
        public void Update_DifferentEntity()
        {
            var entity1 = new BaseTestEntity2 { Name = "test 1" };
            var entity2 = new BaseTestEntity { Name = "test 2" };
            Assert.IsFalse(entity1.Update(entity2));
            Assert.AreEqual("test 1", entity1.Name);
            Assert.AreEqual(null, entity1.LogChangedOn);
        }

        [Test]
        public void Update_Normal()
        {
            var entity1 = new BaseTestEntity { Name = "test 1" };
            var entity2 = new BaseTestEntity { Name = "test 2" };
            var date1 = DateTime.UtcNow;
            Assert.IsTrue(entity1.Update(entity2));
            var date2 = DateTime.UtcNow;
            Assert.IsNotNull(entity1.LogChangedOn);
            Assert.LessOrEqual(date1, entity1.LogChangedOn);
            Assert.GreaterOrEqual(date2, entity1.LogChangedOn);
            Assert.AreEqual("test 2", entity1.Name);
        }

        [Test]
        public void Update_NewData()
        {
            var entity1 = new BaseTestEntity { Name = null };
            var entity2 = new BaseTestEntity { Name = "test 1" };
            var date1 = DateTime.UtcNow;
            Assert.IsTrue(entity1.Update(entity2));
            var date2 = DateTime.UtcNow;
            Assert.IsNotNull(entity1.LogChangedOn);
            Assert.LessOrEqual(date1, entity1.LogChangedOn);
            Assert.GreaterOrEqual(date2, entity1.LogChangedOn);
            Assert.AreEqual("test 1", entity1.Name);
        }

        [Test]
        public void Update_UnchangedArray()
        {
            var arr = new[] { 1, 2, 3 }.ToList();
            var entity1 = new BaseTestEntity { Numbers = arr };
            var entity2 = new BaseTestEntity { Numbers = arr };
            Assert.IsFalse(entity1.Update(entity2));
            Assert.IsNull(entity1.LogChangedOn);
            Assert.AreEqual(arr, entity1.Numbers);
        }

        [Test]
        public void Update_ExtendArray()
        {
            var arr1 = new[] { 1, 2 }.ToList();
            var arr2 = new[] { 1, 2, 3 }.ToList();
            var entity1 = new BaseTestEntity { Numbers = arr1 };
            var entity2 = new BaseTestEntity { Numbers = arr2 };
            var date1 = DateTime.UtcNow;
            Assert.IsTrue(entity1.Update(entity2));
            var date2 = DateTime.UtcNow;
            Assert.IsNotNull(entity1.LogChangedOn);
            Assert.LessOrEqual(date1, entity1.LogChangedOn);
            Assert.GreaterOrEqual(date2, entity1.LogChangedOn);
            Assert.AreEqual(arr2, entity1.Numbers);
        }

        [Test]
        public void Update_ReduceArray()
        {
            var arr1 = new[] { 1, 2, 3 }.ToList();
            var arr2 = new[] { 1, 3 }.ToList();
            var entity1 = new BaseTestEntity { Numbers = arr1 };
            var entity2 = new BaseTestEntity { Numbers = arr2 };
            var date1 = DateTime.UtcNow;
            Assert.IsTrue(entity1.Update(entity2));
            var date2 = DateTime.UtcNow;
            Assert.IsNotNull(entity1.LogChangedOn);
            Assert.LessOrEqual(date1, entity1.LogChangedOn);
            Assert.GreaterOrEqual(date2, entity1.LogChangedOn);
            Assert.AreEqual(arr2, entity1.Numbers);
        }

        [Test]
        public void Update_MissingData()
        {
            var entity1 = new BaseTestEntity { Name = "test 1" };
            var entity2 = new BaseTestEntity { Name = null };
            Assert.IsFalse(entity1.Update(entity2));
            Assert.IsNull(entity1.LogChangedOn);
            Assert.AreEqual("test 1", entity1.Name);
        }

        [Test]
        public void Update_Unchanged()
        {
            var entity1 = new BaseTestEntity { Name = "test 1" };
            var entity2 = new BaseTestEntity { Name = "test 1" };
            Assert.IsFalse(entity1.Update(entity2));
            Assert.AreEqual("test 1", entity1.Name);
            Assert.AreEqual(null, entity1.LogChangedOn);
        }

        private class BaseTestEntity : BaseEntity
        {
            public string Name { get; set; }
            public IEnumerable<int> Numbers { get; set; }
            public DateTime? CreatedOn { get; private set; }

            public BaseTestEntity()
            {
                CreatedOn = LogCreatedOn;
                LogCreatedOn = DateTime.MinValue;
            }
        }

        private class BaseTestEntity2 : BaseEntity
        {
            public string Name { get; set; }

            public BaseTestEntity2()
            {
                LogCreatedOn = DateTime.MinValue;
            }
        }
    }
}
