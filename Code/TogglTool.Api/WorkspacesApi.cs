using System.Collections.Generic;
using System.Linq;
using TogglTool.Api.Database.Repository;
using TogglTool.Api.Models;

namespace TogglTool.Api
{
    public class WorkspacesApi : TogglBaseRepository
    {
        #region .ctor
        public WorkspacesApi(ITogglApi togglApi, IBaseRepository baseRepository, TogglApiMode mode)
            : base(togglApi, baseRepository, mode)
        { }
        #endregion

        public static void AttachClients(Workspace workspace, ICollection<Client> clients)
        {
            foreach (var client in clients)
            {
                var clientId = client.id;
                workspace.ClientList.Add(client);
                client.Workspace = workspace;
                foreach (var project in workspace.ProjectList.Where(x => x.cid == clientId))
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
                var projectId = project.id;
                workspace.ProjectList.Add(project);
                project.Workspace = workspace;
                project.Client = workspace.ClientList.FirstOrDefault(x => x.id == project.cid);
                if (project.Client != null)
                    project.Client.ProjectList.Add(project);
                foreach (var timeEntry in workspace.TimeEntryList.Where(x => x.pid.HasValue && x.pid.Value == projectId))
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

        public List<Workspace> GetWorkspaces(WorkspaceOption option = WorkspaceOption.OnlyWorkspaces)
        {
            var url = "v8/workspaces";
            var query = new Dictionary<string, string>();
            var workspaces = Query(
                togglApi => togglApi.Call<Workspace>(url, query),
                baseRepository => baseRepository.Where<Workspace>(x => true).ToList()
                );
            if ((option & WorkspaceOption.IncludeClients) != 0)
                workspaces.ForEach(x => GetWorkspaceClients(x));
            if ((option & WorkspaceOption.IncludeProjects) != 0)
                workspaces.ForEach(x => GetWorkspaceProjects(x));
            return workspaces;
        }

        public List<Client> GetWorkspaceClients(Workspace workspace)
        {
            var url = string.Format("v8/workspaces/{0}/clients", workspace.id);
            var query = new Dictionary<string, string>();
            var clients = Query(
                togglApi => togglApi.Call<Client>(url, query),
                baseRepository => baseRepository.Where<Client>(x => x.wid == workspace.id).ToList()
                );
            AttachClients(workspace, clients);
            return clients;
        }

        public List<Project> GetWorkspaceProjects(Workspace workspace)
        {
            var url = string.Format("v8/workspaces/{0}/projects", workspace.id);
            var query = new Dictionary<string, string>();
            var projects = Query(
                togglApi => togglApi.Call<Project>(url, query),
                baseRepository => baseRepository.Where<Project>(x => x.wid == workspace.id).ToList()
                );
            AttachProjects(workspace, projects);
            return projects;
        }
    }
}
