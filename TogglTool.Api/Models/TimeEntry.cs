using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TogglTool.Api.Models
{
    public class TimeEntry : BaseEntity
    {
        public int id { get; set; }
        public Guid guid { get; set; }
        public string description { get; set; }
        public bool billable { get; set; }
        public DateTime start { get; set; }
        public DateTime stop { get; set; }
        public int duration { get; set; }
        public string created_with { get; set; }
        public IList<string> tags { get; set; }
        public bool duronly { get; set; }
        public DateTime at { get; set; }

        #region Navigation properties
        public int wid { get; set; }
        public Workspace Workspace { get; set; }

        public int? pid { get; set; }
        public Project Project { get; set; }

        public int? tid { get; set; }
        public int? uid { get; set; }
        #endregion
    }
}
