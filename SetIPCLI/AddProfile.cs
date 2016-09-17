using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SetIPLib;
using System.Net;

namespace SetIPCLI {

    /// <summary>
    /// Expected syntax:
    ///   -a "Profile name" dhcp
    ///   -add "Other name" static 192.168.1.1 255.255.255.0
    /// </summary>
    class AddProfile : ICLICommand {
        private enum ExpectedParameter {
            Name,
            Source,
            IP,
            Sub,
            None
        }

        public CLICommandPriority Priority { get; } = CLICommandPriority.High;

        public ArgumentGroup Arguments { get; }

        public AddProfile(ArgumentGroup args) {
            Arguments = args;
        }

        public void Execute() {
            IProfileStore store = new ProfileFileStore();
            var currentProfiles = store.Retrieve().ToList();
            ExpectedParameter nextParm = ExpectedParameter.Name;

            string name = string.Empty;
            bool UseDHCP = false;
            IPAddress ip = IPAddress.Any;
            IPAddress sub = IPAddress.Any;

            foreach (var parm in Arguments.Arguments) {
                switch (nextParm) {
                    case ExpectedParameter.Name:
                        name = parm;
                        nextParm = ExpectedParameter.Source;
                        break;
                    case ExpectedParameter.Source:
                        if (parm.ToUpper() == "DHCP") {
                            UseDHCP = true;
                            nextParm = ExpectedParameter.None;
                        }
                        else {
                            UseDHCP = false;
                            nextParm = ExpectedParameter.IP;
                        }
                        break;
                    case ExpectedParameter.IP:
                        ip = IPAddress.Parse(parm);
                        nextParm = ExpectedParameter.Sub;
                        break;
                    case ExpectedParameter.Sub:
                        sub = IPAddress.Parse(parm);
                        nextParm = ExpectedParameter.None;
                        break;
                    case ExpectedParameter.None:
                        break;
                }
                if (nextParm == ExpectedParameter.None) {
                    break;
                }
            }
            if (UseDHCP) {
                currentProfiles.Add(new Profile(name));
            }
            else {
                currentProfiles.Add(new Profile(name, ip, sub));
            }

            store.Store(currentProfiles);

        }
    }
}
