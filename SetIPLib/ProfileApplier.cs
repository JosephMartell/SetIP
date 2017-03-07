using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Net;


namespace SetIPLib {

    /// <summary>
    /// This static class is purely a stop-gap measure until something more robust
    /// can be implemented.
    /// </summary>
    public static class ProfileApplier {

        /// <summary>
        /// Applies the provided profile to the designated interface by running
        /// netsh in the background.
        /// </summary>
        /// <param name="interfaceName">The name of the interface to apply the profile to.  
        /// A list of interfaces can be obtained by calling the ListInterfaces method.</param>
        /// <param name="profile">The profile to apply to the named interface.</param>
        public static void ApplyProfile(string interfaceName, Profile profile) {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "netsh";
            if (profile.UseDHCP) {
                startInfo.Arguments = string.Format($"interface ip set address \"{interfaceName}\" dhcp");
            }
            else {
                if (profile.Gateway == IPAddress.None) {
                    startInfo.Arguments = string.Format($"interface ip set address \"{interfaceName}\" static {profile.IP.ToString()} {profile.Subnet.ToString()}");
                }
                else {
                    startInfo.Arguments = string.Format($"interface ip set address \"{interfaceName}\" static {profile.IP.ToString()} {profile.Subnet.ToString()} {profile.Gateway.ToString()}"); 
                }
            }
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            System.IO.StreamReader output;
            using (Process netsh = new Process()) {
                netsh.StartInfo = startInfo;
                netsh.Start();
                output = netsh.StandardOutput;
                netsh.WaitForExit();
            }

            if (profile.DNSServers.Count > 0) {
                //right now, only a single DNS server is supported.  In order to have multiple another netsh command would have to be used:
                //netsh interface ip add dns \"{interfaceName}\" {DNS Address}
                startInfo.Arguments = $"interface ip set dnsservers \"{interfaceName}\" static {profile.DNSServers[0].ToString()}";
            }
            else {
                startInfo.Arguments = string.Format($"interface ip set dnsservers \"{interfaceName}\" dhcp");
            }

            using (Process netsh = new Process()) {
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
    }
}
