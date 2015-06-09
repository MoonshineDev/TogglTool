using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TogglTool.Api.Models
{
    public class Project
    {
        #region .ctor
        public Project()
        {
            TimeEntryList = new List<TimeEntry>();
        }
        #endregion

        public int id { get; set; }
        public Guid? guid { get; set; }
        public string name { get; set; }
        public bool billable { get; set; }
        public bool is_private { get; set; }
        public bool active { get; set; }
        public bool template { get; set; }
        public DateTime at { get; set; }
        public DateTime created_at { get; set; }
        public int color { get; set; }
        public bool auto_estimates { get; set; }
        public int actual_hours { get; set; }

        #region Navigation properties
        public int wid { get; set; }
        public Workspace Workspace { get; set; }

        public int cid { get; set; }
        public Client Client { get; set; }

        public IList<TimeEntry> TimeEntryList { get; set; }
        #endregion
    }
}
