using CLImber;
using System;
using System.Collections;
using System.Linq;

namespace SetIPCLI
{
    [CommandClass("status", ShortDescription = "Displays current status of network adapter.")]
    internal class Status
    {
        private readonly IUserSettings settings;

        public Status(IUserSettings settings)
        {
            this.settings = settings;
        }


        [CommandHandler(ShortDescription = "Displays the configuration of the default target network adapter as designated in the settings file.")]
        public void DefaultAdapterStatus()
        {
            AdapterStatusByName(settings.DefaultNIC);

        }

        [CommandHandler(ShortDescription = "Displayes the configuration of the named network adapter.")]
        public void AdapterStatusByName(string name)
        {
            var nics = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces().Where(n => n.Name == name);

            if (nics.Count() <= 0)
            {
                Console.WriteLine($"Network adapter not found: {name}");
                return;
            }

            var nic = nics.First();
            Console.WriteLine(nic.Name);
            Console.WriteLine(nic.Description);
            Console.WriteLine("");

            var ipProps = nic.GetIPProperties();
            foreach (var addr in ipProps.UnicastAddresses.Where(u => u.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork))
            {
                string type = ipProps.DhcpServerAddresses.Count > 0 ? "(DHCP)" : "(static)";
                Console.WriteLine($"        IP: {addr.Address:20} {type}");
                Console.WriteLine($"    Subnet: {addr.IPv4Mask:20}");
            }

            Console.WriteLine();
            string prefix = "        GW: ";
            foreach (var gw in nic.GetIPProperties().GatewayAddresses)
            {
                Console.WriteLine($"{prefix}{gw.Address:20}");
                prefix = "            ";
            }

            prefix = "       DNS: ";
            foreach (var dns in nic.GetIPProperties().DnsAddresses)
            {
                Console.WriteLine($"{prefix}{dns:20}");
                prefix = "            ";
            }
        }

    }
}
