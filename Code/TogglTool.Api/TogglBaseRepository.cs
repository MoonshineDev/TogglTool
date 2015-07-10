
using System;
using System.Collections.Generic;
using TogglTool.Api.Database.Repository;
using TogglTool.Api.Models;

namespace TogglTool.Api
{
    public abstract class TogglBaseRepository
    {
        private readonly TogglApi _togglApi;
        private readonly IBaseRepository _baseRepository;
        private readonly TogglApiMode _mode;

        protected TogglBaseRepository(TogglApi togglApi, IBaseRepository baseRepository, TogglApiMode mode)
        {
            _togglApi = togglApi;
            _baseRepository = baseRepository;
            _mode = mode;
        }

        protected T Query<T>(Func<TogglApi, T> getOnlineData, Func<IBaseRepository, T> getOfflineData)
            where T : TogglEntity
        {
            switch (_mode)
            {
                case TogglApiMode.Offline:
                    return getOfflineData(_baseRepository);
                case TogglApiMode.Online:
                    return getOnlineData(_togglApi);
                case TogglApiMode.Optimized:
                    var entity = getOfflineData(_baseRepository);
                    // Refresh data if needed
                    if (entity == null || entity.ExpirationOn <= DateTime.UtcNow)
                        entity = getOnlineData(_togglApi);
                    return entity;
                default:
                    return null;
            }
        }

        protected List<T> Query<T>(Func<TogglApi, List<T>> getOnlineData, Func<IBaseRepository, List<T>> getOfflineData)
            where T : TogglEntity
        {
            switch (_mode)
            {
                case TogglApiMode.Offline:
                    return getOfflineData(_baseRepository);
                case TogglApiMode.Online:
                    return getOnlineData(_togglApi);
                case TogglApiMode.Optimized:
                    var list = getOfflineData(_baseRepository);
                    // TODO: Refresh data if needed
                    return list;
                default:
                    return null;
            }
        }
    }
}
