using SetIPLib;
using System.Linq;
using System.Collections.Generic;

namespace SetIPCLI {
    class CLIUseProfile : ICLICommand {

        public ArgumentGroup Arguments { get; }
        public IProfileApplier Applier { get; }

        public string DefaultNIC { get; }

        public CLIUseProfile(ArgumentGroup args, IProfileApplier applier, UserSettings userSettings) {
            Arguments = args;
            Applier = applier;
            DefaultNIC = userSettings.DefaultNIC;
        }

        public void Execute(ref IProfileStore store) {
            string profileName = Arguments.Arguments.DefaultIfEmpty(string.Empty).FirstOrDefault();
            string interfaceName = Arguments.Arguments.Skip(1).DefaultIfEmpty(DefaultNIC).FirstOrDefault();
            var profiles = store.Retrieve();

            //If the supplied profile name is not found, DHCP is used instead.
            Profile target = (from p in profiles
                             where p.Name.ToUpper() == profileName.ToUpper()
                             select p).DefaultIfEmpty(Profile.DHCPDefault).First();

            Applier.Apply(target, interfaceName);
        }

        public string Help() {
            return string.Format(
                "{0, -15}",
                "Use Profile",
                "Usage: setipcli -u \"Profile Name\"\n" +
                "  - profile names without a space do not need to be surrounded by quoatation marks"
                );
        }

        public IEnumerable<string> CommandSummary() {
            return new string[] 
            {
                string.Format(
                    "{0, -15} {1, -30}",
                    "Use Profile",
                    "-u \"Profile Name\" \"Interface Name\"")
            };
        }

    }
}
