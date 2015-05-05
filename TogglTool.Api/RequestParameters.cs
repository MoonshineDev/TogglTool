using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TogglTool.Api
{
    public class RequestParameters
    {
        public string User_Agent { get; set; }
        public int Workspace_Id { get; set; }
        public DateTime SinceDate { get; set; }
        public string Since { get { return SinceDate.ToString("yyyy-MM-dd"); } }
        public DateTime UntilDate { get; set; }
        public string Until { get { return UntilDate.ToString("yyyy-MM-dd"); } }
        // TODO: Add all properties as specified by https://github.com/toggl/toggl_api_docs/blob/master/reports.md#request-parameters
        // TODO: Define default values
        // TODO: Map to url query
    }
}
