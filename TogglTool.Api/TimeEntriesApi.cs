using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TogglTool.Api
{
    public class TimeEntriesApi
    {
        private TogglApi Api { get; set; }

        #region .ctor
        private TimeEntriesApi(TogglApi togglApi)
        {
            Api = togglApi;
        }
        #endregion

        #region Create
        public static TimeEntriesApi Create(TogglApi togglApi)
        {
            if (togglApi == null)
                throw new ArgumentNullException();
            return new TimeEntriesApi(togglApi);
        }
        #endregion
    }
}
