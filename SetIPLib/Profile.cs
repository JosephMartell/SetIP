using System.Net;

namespace SetIPLib {

    public class Profile {
        public string Name { get; } = string.Empty;

        public bool UseDHCP { get; } = true;

        public IPAddress IP { get; } = IPAddress.None;

        public IPAddress Subnet { get; } = IPAddress.None;

        public Profile(string name) {
            Name = name;
        }

        public Profile(string name, IPAddress ip, IPAddress subnet) {
            Name = name;
            IP = ip;
            Subnet = subnet;
            UseDHCP = false;
        }
    }
}
