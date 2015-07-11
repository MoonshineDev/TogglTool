using System.Collections.Generic;
using System.Threading.Tasks;
using TogglTool.Api.Models;

namespace TogglTool.Api
{
    public interface ITogglApi
    {
        List<T> Call<T>(string url, params KeyValuePair<string, string>[] query)
            where T : TogglEntity;

        List<T> Call<T>(string url, IDictionary<string, string> query)
            where T : TogglEntity;

        Task<List<T>> CallAsync<T>(string url, params KeyValuePair<string, string>[] query)
            where T : TogglEntity;

        Task<List<T>> CallAsync<T>(string url, IDictionary<string, string> query)
            where T : TogglEntity;
    }
}
