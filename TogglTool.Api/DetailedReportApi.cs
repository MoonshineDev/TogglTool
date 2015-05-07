using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TogglTool.Api
{
    public class DetailedReportApi
    {
        private TogglApi Api { get; set; }

        #region .ctor
        private DetailedReportApi(TogglApi togglApi)
        {
            Api = togglApi;
        }
        #endregion

        #region Create
        public static DetailedReportApi Create(TogglApi togglApi)
        {
            if (togglApi == null)
                throw new ArgumentNullException();
            return new DetailedReportApi(togglApi);
        }
        #endregion
    }
}
