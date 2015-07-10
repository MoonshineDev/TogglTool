using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TogglTool.Api.Models;

namespace TogglTool.Api
{
    public class TimeEntriesApi : TogglBaseRepository
    {
        #region .ctor
        private TimeEntriesApi(TogglApi togglApi)
            : base(togglApi)
        { }
        #endregion

        #region Create
        public static TimeEntriesApi Create(TogglApi togglApi)
        {
            if (togglApi == null)
                throw new ArgumentNullException();
            return new TimeEntriesApi(togglApi);
        }
        #endregion

        public List<TimeEntry> GetTimeEntries(DateTime start)
        { return GetTimeEntries(start, DateTime.UtcNow); }

        public List<TimeEntry> GetTimeEntries(DateTime start, DateTime end)
        {
            var url = "v8/time_entries";
            var query = new Dictionary<string, string>();
            // 2013-03-10T15:42:46
            query.Add("start_date", start.ToString("yyyy-MM-ddTHH:mm:ssZ"));
            query.Add("end_date", end.ToString("yyyy-MM-ddTHH:mm:ssZ"));
            var timeEntries = Api.Call<TimeEntry>(url, query);
            return timeEntries;
        }
    }
}
