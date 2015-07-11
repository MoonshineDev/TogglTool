using TogglTool.Api.Models;

namespace TogglTool.Api.Export
{
    public interface IExportRepository
    {
        void ExportWorkspace(Workspace workspace);
    }
}
