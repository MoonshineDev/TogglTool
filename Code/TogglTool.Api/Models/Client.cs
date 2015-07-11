using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace TogglTool.Api.Models
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class Client : TogglEntity
    {
        #region ctor
        public Client()
        {
            ProjectList = new List<Project>();
        }
        #endregion

        public string name { get; set; }
        public DateTime at { get; set; }
        public string notes { get; set; }
        public int hrate { get; set; }
        public string cur { get; set; }

        #region Navigation properties
        public int wid { get; set; }
        public Workspace Workspace { get; set; }

        public virtual IList<Project> ProjectList { get; private set; }
        #endregion
    }
}
