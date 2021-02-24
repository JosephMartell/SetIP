using System;
using CLImber;

namespace SetIPCLI
{
    [CommandClass("setting")]
    public class ChangeSettings
    {
        public IUserSettings AppSettings { get; }
        public ChangeSettings(IUserSettings appSettings)
        {
            AppSettings = appSettings;
        }

        [CommandHandler]
        public void ListAllKnownSettings()
        {
            Console.WriteLine($"Valid settings are: \nDefaultNIC\nProfileFileLocation");
        }

        [CommandHandler]
        public void EchoCurrentSetting(string settingName)
        {
            switch (settingName.ToUpper())
            {
                case "DEFAULTNIC":
                    Console.WriteLine(AppSettings.DefaultNIC);
                    break;
                case "PROFILEFILELOCATION":
                    Console.WriteLine(Environment.ExpandEnvironmentVariables(AppSettings.ProfileFileLocation));
                    break;
                default:
                    Console.WriteLine($"Setting name: \"{settingName}\" is not known. Valid settings are: \nDefaultNIC\nProfileFileLocation");
                    break;
            }
        }

        [CommandHandler]
        public void UpdateSetting(string settingName, string newValue)
        {
            switch (settingName.ToUpper())
            {
                case "DEFAULTNIC":
                    AppSettings.DefaultNIC = newValue;
                    break;
                case "PROFILEFILELOCATION":
                    AppSettings.ProfileFileLocation = newValue;
                    break;
                default:
                    Console.WriteLine($"Setting name: \"{settingName}\" is not known. Valid settings are: \nDefaultNIC\nProfileFileLocation");
                    return;
            }
            UserSettings.Default.Save();
        }
    }
}
