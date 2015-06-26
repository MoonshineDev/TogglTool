using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TogglTool.Api
{
    public enum WorkspaceOption
    {
        OnlyWorkspaces = 0,
        IncludeClients = 1,
        IncludeProjects = 2,

        IncludeClientsAndProjects = 3,
    }
}
