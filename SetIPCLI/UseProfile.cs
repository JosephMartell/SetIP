using CLImber;
using SetIPLib;
using System.Linq;

namespace SetIPCLI
{

    [CommandClass("use", ShortDescription = "Applies an IP profile to the current system.")]
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

        [CommandHandler(ShortDescription = "Applies the named profile to the default network adapter.")]
        public void UseProfileByName(string profileName)
        {
            var profiles = Store.Retrieve();

            //If the supplied profile name is not found, DHCP is used instead.
            Profile target = (from p in profiles
                              where p.Name.ToUpper() == profileName.ToUpper()
                              select p).DefaultIfEmpty(Profile.DHCPDefault).First();

            Applier.Apply(target, Settings.DefaultNIC);
        }

        [CommandHandler(ShortDescription = "Sets the default adapter to use DHCP for IP and DNS settings.")]
        public void UseDefaultDHCP()
        {
            Applier.Apply(Profile.DHCPDefault, Settings.DefaultNIC);
        }

        [CommandHandler(ShortDescription = "Applies the named profile to the specified network interface.")]
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