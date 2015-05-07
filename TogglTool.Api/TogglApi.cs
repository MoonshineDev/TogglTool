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

namespace TogglTool.Api
{
    public class TogglApi
    {
        private string ApiKey { get; set; }
        private readonly string _baseurl = "https://www.toggl.com/api/";

        private DetailedReportApi _detailedReport;
        private TimeEntriesApi _timeEntries;
        private WorkspacesApi _workspaces;
        public DetailedReportApi DetailedReport { get {
            if (_detailedReport == null)
                _detailedReport = DetailedReportApi.Create(this);
            return _detailedReport;
        } }
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
        private TogglApi(string apiKey)
        {
            ApiKey = apiKey;
        }
        #endregion

        #region Create
        public static TogglApi Create(string apiKey)
        {
            if (string.IsNullOrEmpty(apiKey))
                throw new ArgumentException();
            return new TogglApi(apiKey);
        }
        #endregion

        public void Call(string url, params KeyValuePair<string, string>[] query)
        {
            var query2 = query.ToDictionary(x => x.Key, x => x.Value);
            Call(url, query2);
        }

        public void Call(string url, IDictionary<string, string> query)
        {
            var task = CallAsync(url, query);
            task.Wait();
        }

        public async Task CallAsync(string url, params KeyValuePair<string, string>[] query)
        {
            var query2 = query.ToDictionary(x => x.Key, x => x.Value);
            await CallAsync(url, query2);
        }

        public async Task CallAsync(string url, IDictionary<string, string> query)
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
                Console.WriteLine("--> {0}", uri);
                var response = await httpClient.GetAsync(uri);
                Console.WriteLine("{0} {1}", response.StatusCode.ToString(), (int)response.StatusCode);
                var content = response.Content;
                var str = await content.ReadAsStringAsync();
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        //var content = response.Content;
                        //var str = await content.ReadAsStringAsync();
                        Console.WriteLine(str);
                        break;
                    default:
                        throw new HttpRequestException("Unhandled HTTP status code " + response.StatusCode);
                }
            }
        }
    }
}
