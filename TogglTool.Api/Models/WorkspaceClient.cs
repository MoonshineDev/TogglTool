using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TogglTool.Api.Models
{
    public class WorkspaceClient
    {
        public int id { get; set; }
        public string name { get; set; }
        public DateTime at { get; set; }
        public string notes { get; set; }
        public int hrate { get; set; }
        public string cur { get; set; }

        #region Navigation properties
        public int wid { get; set; }
        public Workspace Workspace { get; set; }
        #endregion
    }
}
