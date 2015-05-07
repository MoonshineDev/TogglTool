using CommandLine;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TogglTool.Api;

namespace TogglTool.Cli
{
    public class Program
    {
        private readonly Options _options;
        private readonly string _regname = @"Software\TogglTool";
        private readonly string _regkey_togglapikey = @"TogglApiKey";

        #region .ctor
        private Program(Options options)
        {
            _options = options;
        }
        #endregion

        public static void Main(string[] args)
        {
            var parser = Parser.Default;
            var options = new Options();
            var success = default(bool);
            try
            {
                success = parser.ParseArguments(args, options);
                var main = new Program(options);
                main.Run();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Run()
        {
            AssureTogglApiKey();
            var toggl = TogglApi.Create(_options.TogglApiKey);
            var workspacesApi = toggl.Workspaces;
            workspacesApi.GetWorkspaces();
        }

        /// <summary>
        /// Interacts with the windows registry in order to set and retreive the API key for Toggl.
        /// </summary>
        private void AssureTogglApiKey()
        {
            var reg = default(RegistryKey);
            #region Store windows registry values
            if (_options.StoreApiKeys && !string.IsNullOrEmpty(_options.TogglApiKey))
            {
                reg = reg ?? Registry.CurrentUser.CreateSubKey(_regname);
                if (reg != null)
                    reg.SetValue(_regkey_togglapikey, _options.TogglApiKey);
            }
            #endregion
            #region Load windows registry values
            if (string.IsNullOrEmpty(_options.TogglApiKey))
            {
                reg = reg ?? Registry.CurrentUser.OpenSubKey(_regname);
                if (reg != null)
                    _options.TogglApiKey = (string)reg.GetValue(_regkey_togglapikey);
            }
            #endregion
        }
    }
}
