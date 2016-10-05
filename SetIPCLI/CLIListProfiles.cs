﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SetIPLib;

namespace SetIPCLI {
    /// <summary>
    /// Returns a list of all profiles currently stored.
    /// Expected CLI syntax:
    ///   -l
    ///   -list
    /// </summary>
    class CLIListProfiles : ICLICommand {
        public ArgumentGroup Arguments { get; }

        public CLIListProfiles(ArgumentGroup args) {
            Arguments = args;

        }

        public void Execute(ref IProfileStore store) {
            foreach (var profile in store.Retrieve()) {
                if (profile.UseDHCP) {
                    Console.WriteLine("{0,-35} DHCP", profile.Name);
                }
                else {
                    Console.WriteLine("{0,-35} {1,-18} {2,-18}", profile.Name, profile.IP.ToString(), profile.Subnet.ToString());
                }
            }
        }
    }
}