using System.Net;
using System.Collections.Generic;

namespace SetIPLib {

    public class Profile {
        public static Profile DHCPDefault = new Profile("DHCP");
        public string Name { get; } = string.Empty;

        public bool UseDHCP { get; } = true;

        public IPAddress IP { get; } = IPAddress.None;

        public IPAddress Subnet { get; } = IPAddress.None;

        public IPAddress Gateway { get; } = IPAddress.None;

        public List<IPAddress> DNSServers { get; } = new List<IPAddress>();

        public Profile(string name) {
            Name = name;
        }

        public Profile(string name, IPAddress ip, IPAddress subnet) {
            Name = name;
            IP = ip;
            Subnet = subnet;
            UseDHCP = false;
        }

        public Profile(string name, IPAddress ip, IPAddress subnet, IPAddress gateway) {
            Name = name;
            IP = ip;
            Subnet = subnet;
            UseDHCP = false;
            Gateway = gateway;
        }

        public Profile(string name, IPAddress ip, IPAddress subnet, IPAddress gateway, List<IPAddress> dnsServers) {
            Name = name;
            IP = ip;
            Subnet = subnet;
            UseDHCP = false;
            Gateway = gateway;
            DNSServers = dnsServers;
        }

        public ProfileGen Morph() {
            return new ProfileGen(this);
        }

        public class ProfileGen {
            protected Profile _oldProfile;

            public ProfileGen(Profile p) {

            }

            public ProfileGen Name(string newName) {
                if (_oldProfile.UseDHCP) {
                    return new ProfileGen(new Profile(newName));
                }
                else {
                    return new ProfileGen(new Profile(newName, _oldProfile.IP, _oldProfile.Subnet, _oldProfile.Gateway, _oldProfile.DNSServers));
                }
            }

            public ProfileGen IP(IPAddress newIP) {
                return new ProfileGen(new Profile(_oldProfile.Name, newIP, _oldProfile.Subnet, _oldProfile.Gateway, _oldProfile.DNSServers));
            }
        }
    }
}
