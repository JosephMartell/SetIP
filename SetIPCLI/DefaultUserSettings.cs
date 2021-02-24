namespace SetIPCLI
{
    internal class DefaultUserSettings
        : IUserSettings
    {
        public string DefaultNIC
        {
            get
            {
                return UserSettings.Default.DefaultNIC;
            }
            set
            {
                if (value != UserSettings.Default.DefaultNIC)
                {
                    UserSettings.Default.DefaultNIC = value;
                    UserSettings.Default.Save();
                }
            }
        }

        public string ProfileFileLocation
        {
            get
            {
                return UserSettings.Default.ProfileFileLocation;
            }
            set
            {
                if (value != UserSettings.Default.ProfileFileLocation)
                {
                    UserSettings.Default.ProfileFileLocation = value;
                    UserSettings.Default.Save();
                }
            }
        }
    }
}