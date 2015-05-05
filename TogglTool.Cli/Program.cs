using CommandLine;
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
            catch (Exception e)
            {
                throw;
            }
        }

        public void Run()
        {
        }
    }
}
