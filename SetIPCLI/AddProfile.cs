using System.Linq;
using SetIPLib;
using System.Net;
using CLImber;

namespace SetIPCLI
{
    [CommandClass("add")]
    public class AddProfile
    {

        public AddProfile(IProfileStore store)
        {
            Store = store;
        }

        public IProfileStore Store { get; }

        [CommandHandler]
        public void AddDynamicProfile(string profileName)
        {
            var updatedProfiles = Store.Retrieve().Append(Profile.CreateDHCPProfile(profileName));
            Store.Store(updatedProfiles);
        }

        [CommandHandler]
        public void AddStaticProfile(string profileName, IPAddress ip, IPAddress subnetMask)
        {
            var profiles = Store.Retrieve();
            var newProfile = Profile.CreateStaticProfile(profileName, ip, subnetMask);
            Store.Store(profiles.Append(newProfile));
        }

        [CommandHandler]
        public void AddStaticProfile(string profileName, IPAddress ip, IPAddress subnetMask, IPAddress gateway)
        {
            var profiles = Store.Retrieve();
            var newProfile = Profile.CreateStaticProfile(profileName, ip, subnetMask, gateway);
            Store.Store(profiles.Append(newProfile));
        }

        [CommandHandler]
        public void AddStaticProfile(string profileName, IPAddress ip, IPAddress subnetMask, IPAddress gateway, IPAddress DNS)
        {
            var profiles = Store.Retrieve();
            var newProfile = Profile.CreateStaticProfile(profileName, ip, subnetMask, gateway, new IPAddress[] { DNS });
            Store.Store(profiles.Append(newProfile));
        }
    }
}
