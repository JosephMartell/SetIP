using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SetIPLib;

namespace SetIPCLI
{
    class CLIChangeSetting
        : ICLICommand
    {
        public ArgumentGroup Arguments { get; }

        public CLIChangeSetting(ArgumentGroup args)
        {
            Arguments = args;
        }

        public void Execute(ref IProfileStore store)
        {
            if (Arguments.Arguments.Count() == 1)
            {
                DisplaySetting(Arguments.Arguments.First());
            }
            else
            {
                UpdateSetting(Arguments.Arguments.First(), Arguments.Arguments.Skip(1));
            }
        }

        private void DisplaySetting(string settingName)
        {
            Console.WriteLine(Environment.ExpandEnvironmentVariables(UserSettings.Default.ProfileFileLocation));
        }

        private void UpdateSetting(string settingName, IEnumerable<string> parameters)
        {
            UserSettings.Default.ProfileFileLocation = parameters.First();
            UserSettings.Default.Save();
        }

        public IEnumerable<string> CommandSummary()
        {
            string format = "{0, -15} {1, -63}";
            List<string> summary = new List<string>();
            summary.Add(string.Format(format, "Change Setting", "-s SettingName \"Setting Value\""));
            summary.Add(string.Format(format, "", "quotes are only necessary if value contains spaces"));
            summary.Add(string.Format(format, "", "Valid Settings:"));
            summary.Add(string.Format(format, "", "ProfileFileLocation: Any valid file path."));
            summary.Add(string.Format(format, "", "                     System variables are allowed"));
            summary.Add(string.Format(format, "", "                     (e.g.: %APPDATA%)"));

            return summary;
        }

        public string Help()
        {
            throw new NotImplementedException();
        }
    }
}
