using CLImber;
using SetIPLib;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SetIPCLI
{
    [CommandClass("list", ShortDescription = "List all profiles")]
    public class ListProfiles
    {
        public IProfileStore Store { get; }
        public ListProfiles(IProfileStore store)
        {
            Store = store;
        }

        [CommandHandler(ShortDescription = "List all available profiles.")]
        public void ListAllProfiles()
        {
            IEnumerable<Profile> profiles = Store.Retrieve();

            foreach (var profile in profiles.OrderBy(p => p.Name))
            {
                PrintProfile(profile);
            }

        }

        [CommandHandler(ShortDescription = "Lists all profiles with a name that start with the provided filter.")]
        public void ShowFilteredProfileList(string filter)
        {
            IEnumerable<Profile> profiles = Store.Retrieve().Where((p, b) => p.Name.ToUpper().Contains(filter.ToUpper()));
            foreach (var profile in profiles.OrderBy(p => p.Name))
            {
                PrintProfile(profile);
            }

        }

        private void PrintProfile(Profile profile)
        {
            if (profile.UseDHCP)
            {
                Console.WriteLine("{0,-35} DHCP", profile.Name);
            }
            else
            {
                Console.WriteLine("{0,-30} {1,-15} {2,-15} {3,-15}", profile.Name, profile.IP.ToString(), profile.Subnet.ToString(), profile.Gateway.ToString());
            }
        }

    }
}
