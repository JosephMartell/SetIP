using System.Net;

namespace SetIPLib {

    public class Profile {
        public static Profile DHCPDefault = new Profile("DHCP");
        public string Name { get; } = string.Empty;

        public bool UseDHCP { get; } = true;

        public IPAddress IP { get; } = IPAddress.None;

        public IPAddress Subnet { get; } = IPAddress.None;

        public IPAddress Gateway { get; } = IPAddress.None;

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
    }
}
