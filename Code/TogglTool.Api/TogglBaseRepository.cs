
using System;
using System.Collections.Generic;
using TogglTool.Api.Database.Repository;
using TogglTool.Api.Models;

namespace TogglTool.Api
{
    public abstract class TogglBaseRepository
    {
        protected TogglApi Api { get; private set; }
        private IBaseRepository _baseRepository;
        private TogglApiMode _mode;

        protected TogglBaseRepository(TogglApi togglApi, IBaseRepository baseRepository, TogglApiMode mode)
        {
            Api = togglApi;
            _baseRepository = baseRepository;
            _mode = mode;
        }

        protected T Query<T>(Func<TogglApi, T> GetOnlineData, Func<IBaseRepository, T> GetOfflineData)
            where T : TogglEntity
        {
            switch (_mode)
            {
                case TogglApiMode.Offline:
                    return GetOfflineData(_baseRepository);
                case TogglApiMode.Online:
                    return GetOnlineData(Api);
                default:
                    return null;
            }
        }

        protected List<T> Query<T>(Func<TogglApi, List<T>> GetOnlineData, Func<IBaseRepository, List<T>> GetOfflineData)
            where T : TogglEntity
        {
            switch (_mode)
            {
                case TogglApiMode.Offline:
                    return GetOfflineData(_baseRepository);
                case TogglApiMode.Online:
                    return GetOnlineData(Api);
                default:
                    return null;
            }
        }
    }
}
