using System;
using System.Collections.Generic;
using Moq;
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

        [Test]
        public void QuerySingleOnline_NotInDbNotInApi()
        {
            var sut = GetSUT(TogglApiMode.Online);
            var entity = sut.TriggerQuerySingle<TogglFakeEntity>(
                togglApi => null,
                baseRepository => null
                );
            Assert.IsNull(entity);
        }

        [Test]
        public void QuerySingleOnline_NotInDbIsInApi()
        {
            var sut = GetSUT(TogglApiMode.Online);
            var entity = sut.TriggerQuerySingle<TogglFakeEntity>(
                togglApi => new TogglFakeEntity(),
                baseRepository => null
                );
            Assert.IsNotNull(entity);
        }

        private TogglFakeRepository GetSUT(TogglApiMode mode)
        {
            return new TogglFakeRepository(_togglApi.Object, _baseRepository.Object, mode);
        }

        private class TogglFakeEntity : TogglEntity
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
