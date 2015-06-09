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

        public static void AttachClients(Workspace workspace, ICollection<Client> clients)
        {
            foreach (var client in clients)
            {
                workspace.ClientList.Add(client);
                client.Workspace = workspace;
                foreach (var project in workspace.ProjectList.Where(x => x.cid == client.id))
                {
                    client.ProjectList.Add(project);
                    project.Client = client;
                }
            }
        }

        public static void AttachProjects(Workspace workspace, ICollection<Project> projects)
        {
            foreach (var project in projects)
            {
                workspace.ProjectList.Add(project);
                project.Workspace = workspace;
                project.Client = workspace.ClientList.FirstOrDefault(x => x.id == project.cid);
                if (project.Client != null)
                    project.Client.ProjectList.Add(project);
                foreach (var timeEntry in workspace.TimeEntryList.Where(x => x.pid.HasValue && x.pid.Value == project.id))
                {
                    project.TimeEntryList.Add(timeEntry);
                    timeEntry.Project = project;
                }
            }
        }

        public static void AttachTimeEntries(Workspace workspace, ICollection<TimeEntry> timeEntries)
        {
            foreach(var timeEntry in timeEntries)
            {
                workspace.TimeEntryList.Add(timeEntry);
                timeEntry.Workspace = workspace;
                if (timeEntry.pid.HasValue)
                {
                    timeEntry.Project = workspace.ProjectList.FirstOrDefault(x => x.id == timeEntry.pid.Value);
                    if (timeEntry.Project != null)
                        timeEntry.Project.TimeEntryList.Add(timeEntry);
                }
            }
        }

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
            AttachClients(workspace, clients);
            return clients;
        }

        public List<Project> GetWorkspaceProjects(Workspace workspace)
        {
            var url = string.Format("v8/workspaces/{0}/projects", workspace.id);
            var query = new Dictionary<string, string>();
            var projects = Api.Call<List<Project>>(url, query);
            AttachProjects(workspace, projects);
            return projects;
        }
    }
}
