using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SetIPLib;

namespace SetIPCLI {
    class CLIUseProfile : ICLICommand {

        public ArgumentGroup Arguments { get; }

        public void Execute(ref IProfileStore store) {
            string profileName = Arguments.Arguments.DefaultIfEmpty(string.Empty).FirstOrDefault();
            var profiles = store.Retrieve();

            //If the supplied profile name is not found, DHCP is used instead.
            Profile target = (from p in profiles
                             where p.Name == profileName
                             select p).DefaultIfEmpty(Profile.DHCPDefault).First();

            //Eventually, the adapter name will be one of the arguments that can be passed.
            ProfileApplier.ApplyProfile("Local Area Connection", target);
        }

        public CLIUseProfile(ArgumentGroup args) {
            Arguments = args;
        }
    }
}
