using SetIPLib;
using System.Linq;
using System.Collections.Generic;

namespace SetIPCLI {
    class CLIUseProfile : ICLICommand {

        public ArgumentGroup Arguments { get; }
        public IProfileApplier Applier { get; }

        public void Execute(ref IProfileStore store) {
            string profileName = Arguments.Arguments.DefaultIfEmpty(string.Empty).FirstOrDefault();
            string interfaceName = Arguments.Arguments.Skip(1).DefaultIfEmpty("Local Area Connection").FirstOrDefault();
            var profiles = store.Retrieve();

            //If the supplied profile name is not found, DHCP is used instead.
            Profile target = (from p in profiles
                             where p.Name.ToUpper() == profileName.ToUpper()
                             select p).DefaultIfEmpty(Profile.DHCPDefault).First();

            Applier.Apply(target, interfaceName);
        }

        public CLIUseProfile(ArgumentGroup args, IProfileApplier applier) {
            Arguments = args;
            Applier = applier;
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
