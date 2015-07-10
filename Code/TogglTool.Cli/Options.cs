using CommandLine;
using CommandLine.Text;

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
            return HelpText.AutoBuild(this, current => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}
