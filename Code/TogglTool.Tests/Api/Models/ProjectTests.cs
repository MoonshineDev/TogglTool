using NUnit.Framework;
using TogglTool.Api.Models;

namespace TogglTool.Tests.Api.Models
{
    [TestFixture]
    class ProjectTests
    {
        [Test]
        public void Ctor_InitializeArrays()
        {
            var entity = new Project();
            Assert.IsNotNull(entity.TimeEntryList);
            Assert.IsEmpty(entity.TimeEntryList);
        }
    }
}
