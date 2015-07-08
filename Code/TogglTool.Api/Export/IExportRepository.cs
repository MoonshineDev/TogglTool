using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TogglTool.Api.Models;

namespace TogglTool.Api.Export
{
    public interface IExportRepository
    {
        void ExportWorkspace(Workspace workspace);
    }
}
