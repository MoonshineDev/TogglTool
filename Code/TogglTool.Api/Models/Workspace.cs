using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace TogglTool.Api.Models
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class Workspace : TogglEntity
    {
        #region ctor
        public Workspace()
        {
            ClientList = new List<Client>();
            ProjectList = new List<Project>();
            TimeEntryList = new List<TimeEntry>();
        }
        #endregion

        public string name { get; set; }
        public bool premium { get; set; }
        public bool admin { get; set; }
        public int default_hourly_rate { get; set; }
        public string default_currency { get; set; }
        public bool only_admins_may_create_projects { get; set; }
        public bool only_admins_see_billable_rates { get; set; }
        public bool only_admins_see_team_dashboard { get; set; }
        public bool projects_billable_by_default { get; set; }
        public int rounding { get; set; }
        public int rounding_minutes { get; set; }
        public string api_token { get; set; }
        public DateTime at { get; set; }
        public string logo_url { get; set; }
        public bool ical_enabled { get; set; }

        #region Navigation properties
        public virtual IList<Client> ClientList { get; private set; }
        public virtual IList<Project> ProjectList { get; private set; }
        public virtual IList<TimeEntry> TimeEntryList { get; private set; }
        #endregion
    }
}
