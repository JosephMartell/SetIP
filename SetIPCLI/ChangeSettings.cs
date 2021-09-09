using CLImber;
using System;

namespace SetIPCLI
{
    [CommandClass("setting", ShortDescription = "Used to show and update user settings for the SetIP application.")]
    public class ChangeSettings
    {
        public IUserSettings AppSettings { get; }
        public ChangeSettings(IUserSettings appSettings)
        {
            AppSettings = appSettings;
        }

        [CommandHandler(ShortDescription = "Lists all available settings.")]
        public void ListAllKnownSettings()
        {
            Console.WriteLine($"Valid settings are: \nDefaultNIC\nProfileFileLocation");
        }

        [CommandHandler(ShortDescription = "Shows the current value for a setting")]
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

        [CommandHandler(ShortDescription = "Updates a setting with the provided value")]
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
