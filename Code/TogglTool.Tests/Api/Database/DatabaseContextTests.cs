using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TogglTool.Api.Database;
using TogglTool.Api.Models;

namespace TogglTool.Tests.Api.Database
{
    [TestFixture]
    public class DatabaseContextTests
    {
        public DatabaseContext _sut;

        [SetUp]
        public void Setup()
        {
            _sut = new DatabaseContext();
        }

        [Test]
        [Ignore("Not yet implemented")]
        public void ContextOnlyContainsBaseEntity()
        {
        }

        [Test]
        [Ignore("Not yet implemented")]
        public void ContextContainsAllBaseEntity()
        {
        }

        [Test]
        [Ignore("Not yet implemented")]
        public void ContextDoesNotContainArbitraryBaseEntity()
        {
        }

        [Test]
        [Ignore("Not yet implemented")]
        public void TogglEntitiesDoesNotHaveIdentityAttribute()
        {
        }

        private class FakeEntity : BaseEntity
        { }
    }
}
