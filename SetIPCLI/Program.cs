using CLImber;
using SetIPLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace SetIPCLI
{

    class Program
    {
        private static readonly CLIHandler _handler = new CLIHandler();
        static void Main(string[] args)
        {

            string filePath = Environment.ExpandEnvironmentVariables(UserSettings.Default.ProfileFileLocation);
            string directoryPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            IProfileStore store = new StreamProfileStore(
                new FileStream(filePath, FileMode.OpenOrCreate),
                new XMLProfileEncoder());

            _handler.RegisterResource<IProfileStore>(store)
                .RegisterTypeConverter<IPAddress>(s => IPAddress.Parse(s))
                .RegisterResource<IProfileApplier>(new ProfileApplier())
                .RegisterResource<IUserSettings>(new DefaultUserSettings());
            _handler.ProgramDescription = "SetIPCLI is a command line interface to store, document, and apply network configurations. \n" +
                                         "It was designed to help people who routinely travel between multiple locations and work \n" +
                                         "on networks that do not always provide DHCP services.";
            _handler.Handle(args);
        }

    }
}
