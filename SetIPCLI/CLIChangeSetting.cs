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
            if (Arguments.Arguments.Count() == 0)
            {
                CommandSummary().ToList().ForEach(Console.WriteLine);
            }
            else if (Arguments.Arguments.Count() == 1)
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
            switch (settingName.ToUpper())
            {
                case "DEFAULTNIC":
                    Console.WriteLine(UserSettings.Default.DefaultNIC);
                    break;
                case "PROFILEFILELOCATION":
                    Console.WriteLine(Environment.ExpandEnvironmentVariables(UserSettings.Default.ProfileFileLocation));
                    break;
                default:
                    Console.WriteLine($"There is no setting named \"{settingName}\". For a list of valid setting names enter -s with no further commands.");
                    break;
            }
        }

        private void UpdateSetting(string settingName, IEnumerable<string> parameters)
        {
            switch (settingName.ToUpper())
            {
                case "DEFAULTNIC":
                    UserSettings.Default.DefaultNIC = parameters.First();
                    break;
                case "PROFILEFILELOCATION":
                    UserSettings.Default.ProfileFileLocation = parameters.First();
                    break;
                default:
                    Console.WriteLine($"There is no setting named \"{settingName}\". For a list of valid setting names enter -s with no further commands.");
                    return;
            }
            UserSettings.Default.Save();
        }

        public IEnumerable<string> CommandSummary()
        {
            string format = "{0, -15} {1, -63}";
            List<string> summary = new List<string>();
            summary.Add(string.Format(format, "Change Setting", "-s SettingName \"Setting Value\""));
            summary.Add(string.Format(format, "", "quotes are only necessary if value contains spaces"));
            summary.Add(string.Format(format, "", "Valid Setting Names:"));
            summary.Add(string.Format(format, "", "ProfileFileLocation: Any valid file path."));
            summary.Add(string.Format(format, "", "                     System variables are allowed"));
            summary.Add(string.Format(format, "", "                     (e.g.: %APPDATA%)"));
            summary.Add(string.Format(format, "", "DefaultNIC: Default NIC to use when applying profiles."));
            summary.Add(string.Format(format, "", "            Name is not case sensitive."));

            return summary;
        }

        public string Help()
        {
            throw new NotImplementedException();
        }
    }
}
