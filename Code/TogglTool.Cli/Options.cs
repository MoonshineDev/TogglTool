using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TogglTool.Cli
{
    public class Options
    {
        [Option('t', "TogglApiKey", HelpText = "Set the API Key for Toggl API.")]
        public string TogglApiKey { get; set; }

        [Option('s', "StoreApiKeys", HelpText = "Store or update any given API key in windows registry.")]
        public bool StoreApiKeys { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this, (current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}
