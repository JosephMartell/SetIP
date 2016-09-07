using SetIPLib;
using System;
using System.Collections.Generic;
using System.Net;

namespace SetIPCLI {
    class Program {

        private static List<Profile> profiles = new List<Profile>();
        static void Main(string[] args) {
            ProfileFileStore store = new ProfileFileStore();

            profiles.Add(new Profile("Test 1"));
            profiles.Add(new Profile("Test 2", IPAddress.Parse("192.168.1.1"), IPAddress.Parse("255.255.255.0")));
            store.Store(profiles);


            //IEnumerable<Profile> readProfiles = store.Retrieve();
            Console.ReadKey();

        }
    }
}
