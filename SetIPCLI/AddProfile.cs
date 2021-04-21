using CLImber;
using SetIPLib;
using System.Linq;
using System.Net;

namespace SetIPCLI
{
    [CommandClass("add", ShortDescription = "Adds a new IP profile to the storage file")]
    public class AddProfile
    {

        public AddProfile(IProfileStore store)
        {
            Store = store;
        }

        public IProfileStore Store { get; }

        [CommandHandler(ShortDescription = "Creates a new profile with the supplied name that uses DHCP.")]
        public void AddDynamicProfile(string profileName)
        {
            var updatedProfiles = Store.Retrieve().Append(Profile.CreateDHCPProfile(profileName));
            Store.Store(updatedProfiles);
        }

        [CommandHandler(ShortDescription = "Creates a new profile with the supplied name and static IP address/subnet.")]
        public void AddStaticProfile(string profileName, IPAddress ip, IPAddress subnetMask)
        {
            var profiles = Store.Retrieve();
            var newProfile = Profile.CreateStaticProfile(profileName, ip, subnetMask);
            Store.Store(profiles.Append(newProfile));
        }

        [CommandHandler(ShortDescription = "Creates a new profile with the supplied name, static IP address/subnet and the supplied gateway.")]
        public void AddStaticProfile(string profileName, IPAddress ip, IPAddress subnetMask, IPAddress gateway)
        {
            var profiles = Store.Retrieve();
            var newProfile = Profile.CreateStaticProfile(profileName, ip, subnetMask, gateway);
            Store.Store(profiles.Append(newProfile));
        }

        [CommandHandler(ShortDescription = "Creates a new profile with the supplied name, static IP address/subnet, gateway, and DNS address.")]
        public void AddStaticProfile(string profileName, IPAddress ip, IPAddress subnetMask, IPAddress gateway, IPAddress DNS)
        {
            var profiles = Store.Retrieve();
            var newProfile = Profile.CreateStaticProfile(profileName, ip, subnetMask, gateway, new IPAddress[] { DNS });
            Store.Store(profiles.Append(newProfile));
        }
    }
}
