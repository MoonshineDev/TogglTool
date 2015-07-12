using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Core.Extensibility;
using NUnit.Framework;
using TogglTool.Api;
using TogglTool.Api.Database.Repository;
using TogglTool.Api.Models;

namespace TogglTool.Tests.Api
{
    [TestFixture]
    public class TogglBaseRepositoryTests
    {
        private Mock<ITogglApi> _togglApi;
        private Mock<IBaseRepository> _baseRepository;

        [SetUp]
        public void Setup()
        {
            _togglApi = new Mock<ITogglApi>(MockBehavior.Strict);
            _baseRepository = new Mock<IBaseRepository>(MockBehavior.Strict);
        }

        public IEnumerable<TestCaseData> QuerySingleTestCases {
            get
            {
                var dateToday = DateTime.UtcNow;
                var dateYesterday = dateToday.AddDays(-1);
                var dateTomorrow = dateToday.AddDays(1);
                var entities = new List<TogglFakeEntity>
                {
                    null,
                    new TogglFakeEntity { ExpirationOn = null },
                    new TogglFakeEntity { ExpirationOn = dateYesterday },
                    new TogglFakeEntity { ExpirationOn = dateTomorrow },
                };
                foreach (var onlineEntity in entities)
                    foreach (var offlineEntity in entities)
                        yield return new TestCaseData(TogglApiMode.Online, onlineEntity, offlineEntity, true);
                foreach (var onlineEntity in entities)
                    foreach (var offlineEntity in entities)
                        yield return new TestCaseData(TogglApiMode.Offline, onlineEntity, offlineEntity, false);
            }
        }

        [Test]
        [TestCaseSource("QuerySingleTestCases")]
        public void QuerySingle(TogglApiMode mode, TogglFakeEntity onlineResponse, TogglFakeEntity offlineResponse, bool expectedOnline)
        {
            var sut = new TogglFakeRepository(_togglApi.Object, _baseRepository.Object, mode);
            var entity = sut.TriggerQuerySingle(
                togglApi => onlineResponse,
                baseRepository => offlineResponse
                );
            var expected = expectedOnline ? onlineResponse : offlineResponse;
            if (expected == null)
                Assert.IsNull(entity);
            else
            {
                Assert.IsNotNull(entity);
                Assert.AreEqual(expected.id, entity.id);
                Assert.AreEqual(expected.Name, entity.Name);
            }
        }

        public IEnumerable<TestCaseData> QueryListTestCases
        {
            get
            {
                var dateToday = DateTime.UtcNow;
                var dateYesterday = dateToday.AddDays(-1);
                var dateTomorrow = dateToday.AddDays(1);
                var entities = new List<TogglFakeEntity>
                {
                    null,
                    new TogglFakeEntity { ExpirationOn = null },
                    new TogglFakeEntity { ExpirationOn = dateYesterday },
                    new TogglFakeEntity { ExpirationOn = dateTomorrow },
                };
                Func<int[], List<TogglFakeEntity>> getList = ids => ids.Select(x => entities[x]).ToList();
                yield return new TestCaseData(TogglApiMode.Online, null, null, null);
                yield return new TestCaseData(TogglApiMode.Online, null, getList(new[] { 0, 1, 2, 3 }), null);
                yield return new TestCaseData(TogglApiMode.Online, getList(new[] { 0, 1, 2, 3 }), null, getList(new[] { 0, 1, 2, 3 }));
                yield return new TestCaseData(TogglApiMode.Online, getList(new[] { 0, 1, 2, 3 }), getList(new[] { 0, 1, 2, 3 }), getList(new[] { 0, 1, 2, 3 }));
                yield return new TestCaseData(TogglApiMode.Offline, null, null, null);
                yield return new TestCaseData(TogglApiMode.Offline, null, getList(new[] { 0, 1, 2, 3 }), getList(new[] { 0, 1, 2, 3 }));
                yield return new TestCaseData(TogglApiMode.Offline, getList(new[] { 0, 1, 2, 3 }), null, null);
                yield return new TestCaseData(TogglApiMode.Offline, getList(new[] { 0, 1, 2, 3 }), getList(new[] { 0, 1, 2, 3 }), getList(new[] { 0, 1, 2, 3 }));
            }
        }

        [Test]
        [TestCaseSource("QueryListTestCases")]
        public void QueryList(TogglApiMode mode, List<TogglFakeEntity> onlineResponse, List<TogglFakeEntity> offlineResponse, List<TogglFakeEntity> expected)
        {
            var sut = new TogglFakeRepository(_togglApi.Object, _baseRepository.Object, mode);
            var entity = sut.TriggerQueryList(
                togglApi => onlineResponse,
                baseRepository => offlineResponse
                );
            if (expected == null)
                Assert.IsNull(entity);
            else
            {
                Assert.IsNotNull(entity);
                //Assert.AreEqual(expected.id, entity.id);
                //Assert.AreEqual(expected.Name, entity.Name);
            }
        }

        public class TogglFakeEntity : TogglEntity
        {
            public string Name { get; set; }
        }

        private class TogglFakeRepository : TogglBaseRepository
        {
            public TogglFakeRepository(ITogglApi togglApi, IBaseRepository baseRepository, TogglApiMode mode)
                : base(togglApi, baseRepository, mode)
            { }

            public T TriggerQuerySingle<T>(Func<ITogglApi, T> getOnlineData, Func<IBaseRepository, T> getOfflineData)
                where T : TogglEntity
            {
                return Query(getOnlineData, getOfflineData);
            }

            public List<T> TriggerQueryList<T>(Func<ITogglApi, List<T>> getOnlineData, Func<IBaseRepository, List<T>> getOfflineData)
                where T : TogglEntity
            {
                return Query(getOnlineData, getOfflineData);
            }
        }
    }
}
