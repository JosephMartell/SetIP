using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Net;
using System.IO;
using System.Security.Permissions;

namespace SetIPLib {

    public interface IProfileApplier
    {
        void Apply(Profile profile, string interfaceName);
    }

    /// <summary>
    /// This class relies on the Windows program netsh to accomplish the network settings
    /// changes.
    /// </summary>
    public class ProfileApplier : IProfileApplier {

        /// <summary>
        /// Applies the provided profile to the designated interface by running
        /// netsh in the background.
        /// </summary>
        /// <param name="interfaceName">The name of the interface to apply the profile to.  
        /// A list of interfaces can be obtained by calling the ListInterfaces method.</param>
        /// <param name="profile">The profile to apply to the named interface.</param>
        public static void ApplyProfile(string interfaceName, Profile profile)
        {
            SetNicAddress(interfaceName, profile);

            SetDNSServers(interfaceName, profile);
        }

        private static void SetNicAddress(string interfaceName, Profile profile)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo("netsh");
            if (profile.UseDHCP)
                startInfo.Arguments = CreateDHCPNetshArgs(interfaceName);
            else
                startInfo.Arguments = CreateStaticNetshArgs(interfaceName, profile);

            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            StreamReader output;
            using (Process netsh = new Process())
            {
                netsh.StartInfo = startInfo;
                netsh.Start();
                output = netsh.StandardOutput;
                netsh.WaitForExit();
            }
        }

        private const string netshSetAddressPrefix = "interface ip set address ";

        public static string CreateDHCPNetshArgs(string interfaceName)
        {
            return netshSetAddressPrefix + $"\"{interfaceName}\" dhcp";
        }

        public static string CreateStaticNetshArgs(string interfaceName, Profile profile)
        {
            return netshSetAddressPrefix + 
                $"\"{interfaceName}\" static {profile.IP.ToString()} {profile.Subnet.ToString()}" +
                (profile.Gateway == IPAddress.None ? "" : $" {profile.Gateway.ToString()}");
        }

        private static void SetDNSServers(string interfaceName, Profile profile)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo("netsh");
            if (profile.DNSServers.Count > 0)
            {
                //right now, only a single DNS server is supported.  In order to have multiple another netsh command would have to be used:
                //netsh interface ip add dns \"{interfaceName}\" {DNS Address}
                startInfo.Arguments = $"interface ip set dnsservers \"{interfaceName}\" static {profile.DNSServers[0].ToString()}";
            }
            else
            {
                startInfo.Arguments = string.Format($"interface ip set dnsservers \"{interfaceName}\" dhcp");
            }
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            StreamReader output;
            using (Process netsh = new Process())
            {
                netsh.StartInfo = startInfo;
                netsh.Start();
                output = netsh.StandardOutput;
                netsh.WaitForExit();
            }
        }

        /// <summary>
        /// Returns a list of interface names that a profile can be applied to. They are
        /// not guaranteed to be valid and not all valid interfaces may be listed.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<string> ListInterfaces() {
            NetworkInterfaceType validTypes = NetworkInterfaceType.Ethernet |
                                              NetworkInterfaceType.Wireless80211 |
                                              NetworkInterfaceType.Ethernet3Megabit |
                                              NetworkInterfaceType.FastEthernetFx |
                                              NetworkInterfaceType.FastEthernetT |
                                              NetworkInterfaceType.GigabitEthernet;

            var interfaces = (from ifc in NetworkInterface.GetAllNetworkInterfaces()
                              where (ifc.NetworkInterfaceType | validTypes) == validTypes
                              select ifc.Name).ToList();

            return interfaces;

        }

        public void Apply(Profile profile, string interfaceName)
        {
            ApplyProfile(interfaceName, profile);
        }
    }
}
