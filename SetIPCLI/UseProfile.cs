using SetIPLib;
using System.Linq;
using System.Collections.Generic;
using CLImber;
using System;

namespace SetIPCLI
{

    [CommandClass("use")]
    public class UseProfile
    {
        public IProfileStore Store { get; }
        public IProfileApplier Applier { get; }
        public IUserSettings Settings { get; }

        public UseProfile(IProfileStore store, IProfileApplier applier, IUserSettings settings)
        {
            Store = store;
            Applier = applier;
            Settings = settings;
        }

        [CommandHandler]
        public void UseProfileByName(string profileName)
        {
            var profiles = Store.Retrieve();

            //If the supplied profile name is not found, DHCP is used instead.
            Profile target = (from p in profiles
                              where p.Name.ToUpper() == profileName.ToUpper()
                              select p).DefaultIfEmpty(Profile.DHCPDefault).First();

            Applier.Apply(target, Settings.DefaultNIC);
        }

        [CommandHandler]
        public void UseDefaultDHCP()
        {
            Applier.Apply(Profile.DHCPDefault, Settings.DefaultNIC);
        }

        [CommandHandler]
        public void UseProfileOnNIC(string profileName, string interfaceName)
        {
            var profiles = Store.Retrieve();

            //If the supplied profile name is not found, DHCP is used instead.
            Profile target = (from p in profiles
                              where p.Name.ToUpper() == profileName.ToUpper()
                              select p).DefaultIfEmpty(Profile.DHCPDefault).First();

            Applier.Apply(target, interfaceName);
        }
    }
}