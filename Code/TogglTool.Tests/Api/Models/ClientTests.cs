using NUnit.Framework;
using TogglTool.Api.Models;

namespace TogglTool.Tests.Api.Models
{
    [TestFixture]
    public class ClientTests
    {
        [Test]
        public void Ctor_InitializeArrays()
        {
            var entity = new Client();
            Assert.IsNotNull(entity.ProjectList);
            Assert.IsEmpty(entity.ProjectList);
        }
    }
}
