using SetIPLib;
using System.Linq;

namespace SetIPCLI {
    class CLIUseProfile : ICLICommand {

        public ArgumentGroup Arguments { get; }

        public void Execute(ref IProfileStore store) {
            string profileName = Arguments.Arguments.DefaultIfEmpty(string.Empty).FirstOrDefault();
            var profiles = store.Retrieve();

            //If the supplied profile name is not found, DHCP is used instead.
            Profile target = (from p in profiles
                             where p.Name.ToUpper() == profileName.ToUpper()
                             select p).DefaultIfEmpty(Profile.DHCPDefault).First();

            //Eventually, the adapter name will be one of the arguments that can be passed.
            ProfileApplier.ApplyProfile("Local Area Connection", target);
        }

        public CLIUseProfile(ArgumentGroup args) {
            Arguments = args;
        }

        public string Help() {
            return "Usage: setipcli -u \"Profile Name\"\n" +
                   "  - profile names without a space do not need to be surrounded by quoatation marks";
        }
    }
}
