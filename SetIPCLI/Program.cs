using SetIPLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Diagnostics;

namespace SetIPCLI {

    public enum Flags {
        CommandList = 1
    }

    class Program {
        static void Main(string[] args) {
            var argGroups = ArgumentGroup.ParseArguments(args);
            Flags flags;

            flags = ParseFlags(argGroups);

            var commands = CLICommandFactory.GetCommands(argGroups,
                new CLIListCommands(ArgumentGroup.EmptyGroup));

            string filePath = Environment.ExpandEnvironmentVariables(UserSettings.Default.ProfileFileLocation);
            string directoryPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            IProfileStore store = new StreamProfileStore(
                new FileStream(filePath, FileMode.OpenOrCreate), 
                new XMLProfileEncoder());
            foreach (var c in commands) {
                try {
                    c.Execute(ref store);
                }
                catch (UnknownCommandException e) {
                    Console.WriteLine(e.Message);
                }
                catch (System.Xml.XmlException e)
                {
                    Console.WriteLine(e.Message);
                    if (e.InnerException != null)
                        Console.WriteLine($"{e.InnerException.Message}");
                }
            }
        }


        static Flags ParseFlags(IEnumerable<ArgumentGroup> argGroups) {
            return Flags.CommandList;
        }

        //TEST METHODS - not used in normal execution
        //These should be moved to a test project.
        
        //private static ProfileFileStore store = new ProfileFileStore();

        //static void TestStorage() {
        //    List<Profile> profiles = new List<Profile>();
        //    profiles.Add(new Profile("Test 1"));
        //    profiles.Add(new Profile("Test 2", IPAddress.Parse("192.168.1.1"), IPAddress.Parse("255.255.255.0")));
        //    store.Store(profiles);
        //}

        //static void TestRetrieval() {
        //    IEnumerable<Profile> readProfiles = store.Retrieve();
        //}

        //static void TestInterfaceList() {
        //    var interfaces = ProfileApplier.ListInterfaces();
        //    foreach (var i in interfaces) {
        //        Console.WriteLine(i);
        //    }
        //}

        //static void TestApply() {
        //    Profile pStatic = new Profile("Test Static",
        //                            IPAddress.Parse("10.10.10.50"),
        //                            IPAddress.Parse("255.255.0.0"));
        //    ProfileApplier.ApplyProfile("Wireless Network Connection", pStatic);

        //    System.Threading.Thread.Sleep(20000);

        //    Profile pDynamic = new Profile("Test Dynamic");
        //    ProfileApplier.ApplyProfile("Wireless Network Connection", pDynamic);
        //}
    }
}
