using CLImber;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;

namespace SetIPCLI
{
    [CommandClass("adapters", ShortDescription = "Lists all available network adapters on the current system.")]
    internal class Adapters
    {
        [CommandOption("case", Abbreviation = 'c', Description = "If set, will perform a case sensitive match on adapter names.")]
        public bool CaseSensitive { get; set; }

        [CommandHandler(ShortDescription = "Lists all adapters.")]
        public void List()
        {
            var nics = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces();

            foreach (var nic in nics)
            {
                Console.WriteLine($"   {nic.Name}");
            }
        }
        [CommandHandler(ShortDescription = "Filters the list of returned adapters.")]
        public void List(string filter)
        {
            IEnumerable<NetworkInterface> nics;
            if (CaseSensitive)
            {
                nics = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces().Where(a => a.Name.Contains(filter));
            }
            else
            {
                nics = System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces().Where(a => a.Name.ToUpper().Contains(filter.ToUpper()));
            }

            foreach (var nic in nics)
                {
                    Console.WriteLine($"   {nic.Name}");
                }
        }
    }
}
