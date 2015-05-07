using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TogglTool.Api
{
    public class WorkspacesApi
    {
        private TogglApi Api { get; set; }

        #region .ctor
        private WorkspacesApi(TogglApi togglApi)
        {
            Api = togglApi;
        }
        #endregion

        #region Create
        public static WorkspacesApi Create(TogglApi togglApi)
        {
            if (togglApi == null)
                throw new ArgumentNullException();
            return new WorkspacesApi(togglApi);
        }
        #endregion

        public void GetWorkspaces()
        {
            var url = "v8/workspaces";
            var query = new Dictionary<string, string>();
            Api.Call(url, query);
        }
    }
}
