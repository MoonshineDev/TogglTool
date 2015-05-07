using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TogglTool.Api
{
    public class RequestParameters
    {
        // TODO: Modify properties as specified by https://github.com/toggl/toggl_api_docs/blob/master/reports.md#request-parameters
        public string User_Agent { get; set; }
        public int Workspace_Id { get; set; }
        public DateTime _since;
        public string Since { get { return _since.ToString("yyyy-MM-dd"); } }
        public DateTime _until;
        public string Until { get { return _until.ToString("yyyy-MM-dd"); } }
        public string Billable { get; set; }
        public object Client_Ids { get; set; }
        public object Project_Ids { get; set; }
        public object User_Ids { get; set; }
        public object Tag_Ids { get; set; }
        public object Task_Ids { get; set; }
        public object Time_Entry { get; set; }
        public string Description { get; set; }
        public bool? Without_Description { get; set; }
        public object Order_Field { get; set; }
        public bool? Order_Desc { get; set; }
        public bool _distinctRates;
        public string Distinct_Rates { get { return _distinctRates ? "on" : "off"; } }
        public bool _rounding;
        public string Rounding { get { return _rounding ? "on" : "off"; } }
        public object Display_Hours { get; set; }

        public static RequestParameters Create(string userAgent, int workspaceId)
        {
            var req = Default();
            req.User_Agent = userAgent;
            req.Workspace_Id = workspaceId;
            return req;
        }

        private static RequestParameters Default()
        {
            // TODO: Define default values
            var now = DateTime.Now;
            return new RequestParameters {
                _since = now.AddDays(-6),
                _until = now,
                Billable = "both",
                _distinctRates = false,
                _rounding = false,
            };
        }

        public IDictionary<string, string> ToDictionary()
        {
            return GetType()
                .GetProperties()
                .ToDictionary(
                    x => x.Name.ToLower(),
                    x => x.GetValue().ToString(),
                );
        }
    }
}
