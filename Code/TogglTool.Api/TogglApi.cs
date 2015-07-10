using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using TogglTool.Api.Database.Repository;
using TogglTool.Api.Models;

namespace TogglTool.Api
{
    public class TogglApi
    {
        private string ApiKey { get; set; }
        private readonly string _baseurl = "https://www.toggl.com/api/";

        public string UserAgent { get; private set; }

        private IBaseRepository _baseRepository;
        private TogglApiMode _mode;
        private TimeEntriesApi _timeEntries;
        private WorkspacesApi _workspaces;
        public TimeEntriesApi TimeEntries { get {
            if (_timeEntries == null)
                _timeEntries = TimeEntriesApi.Create(this);
            return _timeEntries;
        } }
        public WorkspacesApi Workspaces { get {
            if (_workspaces == null)
                _workspaces = WorkspacesApi.Create(this);
            return _workspaces;
        } }

        #region .ctor
        private TogglApi(string apiKey, string userAgent, IBaseRepository baseRepository, TogglApiMode mode)
        {
            ApiKey = apiKey;
            UserAgent = userAgent;
            _baseRepository = baseRepository;
            _mode = mode;
        }
        #endregion

        #region Create
        public static TogglApi Create(string apiKey, string userAgent, IBaseRepository baseRepository, TogglApiMode mode = TogglApiMode.Online)
        {
            if (string.IsNullOrEmpty(apiKey))
                throw new ArgumentException("apiKey");
            if (string.IsNullOrEmpty(userAgent))
                throw new ArgumentException("userAgent");
            return new TogglApi(apiKey, userAgent, baseRepository, mode);
        }
        #endregion

        public List<T> Call<T>(string url, params KeyValuePair<string, string>[] query)
            where T : TogglEntity
        {
            var query2 = query.ToDictionary(x => x.Key, x => x.Value);
            return Call<T>(url, query2);
        }

        public List<T> Call<T>(string url, IDictionary<string, string> query)
            where T : TogglEntity
        {
            var task = CallAsync<T>(url, query);
            task.Wait();
            return task.Result;
        }

        public async Task<List<T>> CallAsync<T>(string url, params KeyValuePair<string, string>[] query)
            where T : TogglEntity
        {
            var query2 = query.ToDictionary(x => x.Key, x => x.Value);
            return await CallAsync<T>(url, query2);
        }

        public async Task<List<T>> CallAsync<T>(string url, IDictionary<string, string> query)
            where T : TogglEntity
        {
            url = _baseurl + Regex.Replace(url, @"^[\/]*", "");
            var uriBuilder = new UriBuilder(url);
            var q = HttpUtility.ParseQueryString(uriBuilder.Query);
            foreach (var kvp in query)
                q[kvp.Key] = kvp.Value;
            uriBuilder.Query = q.ToString();
            var uri = uriBuilder.Uri;
            using (var httpClient = new HttpClient())
            {
                var byteArray = Encoding.ASCII.GetBytes(ApiKey + ":api_token");
                var authHeader = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                httpClient.DefaultRequestHeaders.Authorization = authHeader;
                var response = await httpClient.GetAsync(uri);
                var code = response.StatusCode;
                var content = default(string);
                Console.WriteLine("{0} {1} {2}", (int)code, code.ToString(), uri);
                switch (code)
                {
                    case HttpStatusCode.OK:
                        content = await response.Content.ReadAsStringAsync();
                        var list = JsonConvert.DeserializeObject<List<T>>(content);
                        _baseRepository.AddOrUpdate(list);
                        _baseRepository.SaveChanges();
                        return list;
                    default:
                        content = await response.Content.ReadAsStringAsync();
                        throw new HttpRequestException("Unhandled HTTP status code " + (int)code + " " + response.StatusCode);
                }
            }
        }
    }
}
