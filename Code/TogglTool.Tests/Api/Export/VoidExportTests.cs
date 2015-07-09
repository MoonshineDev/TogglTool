using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TogglTool.Api.Export;
using TogglTool.Api.Models;

namespace TogglTool.Tests.Api.Export
{
    [TestFixture]
    public class VoidExportTests
    {
        private IExportRepository _sut;

        [SetUp]
        public void Setup()
        {
            _sut = new VoidExport();
        }

        public IEnumerable<Workspace> ExportWorkspace_TestCases
        {
            get
            {
                var r = new Random();
                yield return default(Workspace);
                yield return new Workspace { };
                yield return new Workspace { id = 1 };
                yield return new Workspace { id = 0 };
                yield return new Workspace { id = -1 };
                yield return new Workspace { id = int.MaxValue };
                yield return new Workspace { id = int.MinValue };
                yield return new Workspace { id = r.Next(int.MinValue, -1) };
                yield return new Workspace { id = r.Next(1, int.MaxValue) };
            }
        }

        [Test]
        [TestCaseSource("ExportWorkspace_TestCases")]
        public void ExportWorkspace(Workspace workspace)
        {
            _sut.ExportWorkspace(workspace);
        }
    }
}
