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

        public List<Workspace> GetWorkspaces(bool includeClients = false, bool includeProjects = false)
        {
            var url = "v8/workspaces";
            var query = new Dictionary<string, string>();
            var workspaces = Api.Call<List<Workspace>>(url, query);
            if (includeClients)
                workspaces.ForEach(x => GetWorkspaceClients(x));
            if (includeProjects)
                workspaces.ForEach(x => GetWorkspaceProjects(x));
            return workspaces;
        }

        public List<Client> GetWorkspaceClients(Workspace workspace)
        {
            var url = string.Format("v8/workspaces/{0}/clients", workspace.id);
            var query = new Dictionary<string, string>();
            var clients = Api.Call<List<Client>>(url, query);
            clients.ForEach(x => {
                workspace.ClientList.Add(x);
                x.Workspace = workspace;
                foreach (var project in workspace.ProjectList.Where(y => y.cid == x.id))
                {
                    x.ProjectList.Add(project);
                    project.Client = x;
                }
            });
            return clients;
        }

        public List<Project> GetWorkspaceProjects(Workspace workspace)
        {
            var url = string.Format("v8/workspaces/{0}/projects", workspace.id);
            var query = new Dictionary<string, string>();
            var projects = Api.Call<List<Project>>(url, query);
            projects.ForEach(x => {
                workspace.ProjectList.Add(x);
                x.Workspace = workspace;
                x.Client = workspace.ClientList.FirstOrDefault(y => y.id == x.cid);
                if (x.Client != null)
                    x.Client.ProjectList.Add(x);
            });
            return projects;
        }
    }
}
