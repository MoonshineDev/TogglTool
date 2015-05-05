using CommandLine;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var togglApiKey = _options.TogglApiKey;
            var reg = default(RegistryKey);
            #region Store windows registry values
            if (_options.StoreApiKeys && !string.IsNullOrEmpty(togglApiKey))
            {
                reg = reg ?? Registry.CurrentUser.CreateSubKey(_regname);
                if (reg != null)
                {
                    reg.SetValue(_regkey_togglapikey, togglApiKey);
                }
            }
            #endregion
            #region Load windows registry values
            if (string.IsNullOrEmpty(togglApiKey))
            {
                reg = reg ?? Registry.CurrentUser.OpenSubKey(_regname);
                if (reg != null)
                {
                    var val = reg.GetValue(_regkey_togglapikey);
                }
            }
            #endregion
        }
    }
}
