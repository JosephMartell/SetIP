using SetIPLib;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Diagnostics;

namespace SetIPCLI {
    class Program {

        private static ProfileFileStore store = new ProfileFileStore();

        static void Main(string[] args) {
            //TestStorage();
            //TestRetrieval();
            //TestInterfaceList();
            TestApply();
            Console.Write("Press any key...");
            Console.ReadKey();
        }

        static void TestStorage() {
            List<Profile> profiles = new List<Profile>();
            profiles.Add(new Profile("Test 1"));
            profiles.Add(new Profile("Test 2", IPAddress.Parse("192.168.1.1"), IPAddress.Parse("255.255.255.0")));
            store.Store(profiles);
        }

        static void TestRetrieval() {
            IEnumerable<Profile> readProfiles = store.Retrieve();
        }

        static void TestInterfaceList() {
            var interfaces = ProfileApplier.ListInterfaces();
            foreach (var i in interfaces) {
                Console.WriteLine(i);
            }
        }

        static void TestApply() {
            Profile pStatic = new Profile("Test Static",
                                    IPAddress.Parse("10.10.10.50"),
                                    IPAddress.Parse("255.255.0.0"));
            ProfileApplier.ApplyProfile("Wireless Network Connection", pStatic);

            System.Threading.Thread.Sleep(20000);

            Profile pDynamic = new Profile("Test Dynamic");
            ProfileApplier.ApplyProfile("Wireless Network Connection", pDynamic);
        }
    }
}
