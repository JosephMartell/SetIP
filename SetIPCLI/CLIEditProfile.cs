using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SetIPLib;
using System.Threading.Tasks;
using System.Net;

namespace SetIPCLI {
    class CLIEditProfile : ICLICommand {
        private enum ExpectedParameter {
            OriginalName,
            Name,
            Source,
            IP,
            Sub,
            Gateway,
            DNS,
            None
        }

        public ArgumentGroup Arguments { get; }
        public CLIEditProfile(ArgumentGroup args) {
            Arguments = args;
        }

        public void Execute(ref IProfileStore store) {
            var currentProfiles = store?.Retrieve().ToList();

            var nextParm = ExpectedParameter.OriginalName;
            
            //first parameter must be the original name
            string originalProfileName = string.Empty;
            string newName = null;
            Nullable<bool> useDHCP = null;
            IPAddress newIP = null;
            IPAddress newSub = null;
            IPAddress newGW = null;
            List<IPAddress> newDNS = null;
            foreach (var arg in Arguments.Arguments) {
                if (arg.Contains("=")) {
                    nextParm = ParseArgument(arg);
                }

                switch (nextParm) {
                    case ExpectedParameter.OriginalName:
                        originalProfileName = GetValueFromArg(arg);
                        nextParm = ExpectedParameter.Name;
                        break;
                    case ExpectedParameter.Name:
                        newName = GetValueFromArg(arg);
                        nextParm = ExpectedParameter.Source;
                        break;
                    case ExpectedParameter.Source:
                        useDHCP = GetValueFromArg(arg).ToLower().Contains("true");
                        nextParm = ExpectedParameter.IP;
                        break;
                    case ExpectedParameter.IP:
                        newIP = IPAddress.Parse(GetValueFromArg(arg));
                        nextParm = ExpectedParameter.Sub;
                        break;
                    case ExpectedParameter.Sub:
                        newSub = IPAddress.Parse(GetValueFromArg(arg));
                        nextParm = ExpectedParameter.Gateway;
                        break;
                    case ExpectedParameter.Gateway:
                        newGW = IPAddress.Parse(GetValueFromArg(arg));
                        nextParm = ExpectedParameter.DNS;
                        break;
                    case ExpectedParameter.DNS:
                        if (newDNS == null) {
                            newDNS = new List<IPAddress>();
                        }
                        newDNS.Add(IPAddress.Parse(GetValueFromArg(arg)));
                        nextParm = ExpectedParameter.None;
                        break;
                    default:
                        nextParm = ExpectedParameter.None;
                        break;
                }
            }

            Profile originalProfile = currentProfiles.Find(p => p.Name.ToLower() == originalProfileName.ToLower());
            if (!useDHCP.HasValue) {
                useDHCP = originalProfile.UseDHCP;
            }

            Profile newProfile = null;
            if (useDHCP.HasValue) {
                if (useDHCP.Value) {
                    newProfile = new Profile(newName ?? originalProfile.Name);
                }
                else {
                    newProfile = new Profile(newName ?? originalProfile.Name,
                                             newIP ?? originalProfile.IP,
                                             newSub ?? originalProfile.Subnet,
                                             newGW ?? originalProfile.Gateway,
                                             newDNS ?? originalProfile.DNSServers);
                }
            }

            currentProfiles.Insert(currentProfiles.IndexOf(originalProfile), newProfile);
            currentProfiles.Remove(originalProfile);
            store.Store(currentProfiles);
        }

        private ExpectedParameter ParseArgument(string text) {
            var s = text.Split('=');
            if (s.Length > 1) {
                string type = s[0].ToLower().Trim();
                switch (type) {
                    case "originalname":
                        return ExpectedParameter.OriginalName;
                    case "name":
                        return ExpectedParameter.Name;
                    case "source":
                        return ExpectedParameter.Source;
                    case "ip":
                        return ExpectedParameter.IP;
                    case "sub":
                    case "subnet":
                        return ExpectedParameter.Sub;
                    case "gateway":
                    case "gw":
                        return ExpectedParameter.Gateway;
                    case "dns":
                        return ExpectedParameter.DNS;
                    default:
                        return ExpectedParameter.None;
                }
            }
            return ExpectedParameter.None;
        }

        private string GetValueFromArg(string text) {
            if (text.Contains("=")) {
                return text.Substring(text.LastIndexOf('=')+1).Trim();
            }
            else {
                return text;
            }
        }

        public string Help() {
            //               1         2         3         4         5         6         7        8
            //      123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890
            return "Usage: setipcli -e [originalname=]\"Original Profile Name\" name=new-value\n" +
                   "  - If the name originalname is not explicitly used then the first parameter\n" +
                   "    MUST be the original profile name\n" +
                   "  - The following property names are recognized:\n" +
                   "         + originalname - Identifies the original profile to be modified\n" +
                   "         + name - specifies a new name for the profile\n" +
                   "         + source - valid values are \"DHCP\" or \"static\"\n" +
                   "         + ip - specifies a new IP address in decimal-dot notation\n" +
                   "         + sub/subnet - Specifies a new subnet mask in decimal-dot notation\n" +
                   "         + gateway/gw - Specifies a new default gateway\n" +
                   "         + dns - Can be listed multiple times to specify a new list of DNS\n" +
                   "                 All previous DNS listings are removed";
        }
    }
}
