using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TogglTool.Api.Models;

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

        public List<Workspace> GetWorkspaces(bool includeClients = false)
        {
            var url = "v8/workspaces";
            var query = new Dictionary<string, string>();
            var workspaces = Api.Call<List<Workspace>>(url, query);
            if (includeClients)
                workspaces.ForEach(x => GetWorkspaceClients(x));
            return workspaces;
        }

        public List<WorkspaceClient> GetWorkspaceClients(Workspace workspace)
        {
            var url = string.Format("v8/workspaces/{0}/clients", workspace.id);
            var query = new Dictionary<string, string>();
            var workspaceClients = Api.Call<List<WorkspaceClient>>(url, query);
            workspace.WorkspaceClientList = workspaceClients;
            workspaceClients.ForEach(x => x.Workspace = workspace);
            return workspaceClients;
        }
    }
}
