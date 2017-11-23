using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;

namespace SetIPLib {

    public class Profile :
        IComparable<Profile>,
        IEquatable<Profile> {
        public static Profile DHCPDefault = new Profile("DHCP");
        public string Name { get; } = string.Empty;

        public bool UseDHCP { get; } = true;

        public IPAddress IP { get; } = IPAddress.None;

        public IPAddress Subnet { get; } = IPAddress.None;

        public IPAddress Gateway { get; } = IPAddress.None;

        public List<IPAddress> DNSServers { get; } = new List<IPAddress>();

        //TODO: refactor this to use well-named factory methods
        public static Profile CreateDHCPProfile(string name)
        {
            return new Profile(name);
        }

        private Profile(string name)
        {
            Name = name;
        }

        public static Profile CreateStaticProfile(string name, IPAddress ip, IPAddress subnet)
        {
            return new Profile(name, ip, subnet);
        }

        public static Profile CreateStaticProfile(string name, IPAddress ip, IPAddress subnet, IPAddress gateway)
        {
            return new Profile(name, ip, subnet, gateway);
        }

        public static Profile CreateStaticProfile(string name, IPAddress ip, IPAddress subnet, IPAddress gateway, IEnumerable<IPAddress> dnsServers)
        {
            return new Profile(name, ip, subnet, gateway, dnsServers.ToList());
        }

        private Profile(string name, IPAddress ip, IPAddress subnet)
        {
            Name = name;
            IP = ip;
            Subnet = subnet;
            UseDHCP = false;
        }

        private Profile(string name, IPAddress ip, IPAddress subnet, IPAddress gateway)
        {
            Name = name;
            IP = ip;
            Subnet = subnet;
            UseDHCP = false;
            Gateway = gateway;
        }

        private Profile(string name, IPAddress ip, IPAddress subnet, IPAddress gateway, List<IPAddress> dnsServers)
        {
            Name = name;
            IP = ip;
            Subnet = subnet;
            UseDHCP = false;
            Gateway = gateway;
            DNSServers = dnsServers;
        }

        public int CompareTo(Profile other)
        {
            return Name.CompareTo(other.Name);
        }

        public bool Equals(Profile other)
        {
            bool AreEqual = (Name == other.Name);
            AreEqual &= (UseDHCP == other.UseDHCP);
            AreEqual &= IP.Equals(other.IP);
            AreEqual &= Subnet.Equals(other.Subnet);
            AreEqual &= Gateway.Equals(other.Gateway);
            AreEqual &= DNSServersAreEqual(other.DNSServers);
            return AreEqual;
        }

        private bool DNSServersAreEqual(List<IPAddress> DNSServers)
        {
            if (this.DNSServers.Count != DNSServers.Count)
            {
                return false;
            }

            for (int i = 0; i < this.DNSServers.Count; i++)
            {
                if (!this.DNSServers[i].Equals(DNSServers[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Profile otherP = (Profile)obj;
            return Equals(otherP);
        }
    }
}
