
using System;

namespace TogglTool.Api
{
    [Flags]
    public enum WorkspaceOption
    {
        OnlyWorkspaces = 0,
        IncludeClients = 1,
        IncludeProjects = 2,

        IncludeClientsAndProjects = 3,
    }
}
