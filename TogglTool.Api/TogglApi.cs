using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TogglTool.Api
{
    public class TogglApi
    {
        private string ApiKey { get; set; }
        private readonly string _baseurl = "https://www.toggl.com/api/";

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
    }
}
