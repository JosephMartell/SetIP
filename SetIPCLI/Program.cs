using SetIPLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Diagnostics;
using CLImber;
using System.Linq;

namespace SetIPCLI {

    public class ArgToIPConverter
        : IArgumentTypeConverter
    {
        public object ConvertArgument(string arg)
        {
            return IPAddress.Parse(arg);
        }
    }

    class Program {
        private static CLIHandler _handler = new CLIHandler();
        static void Main(string[] args) {

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
                .RegisterTypeConverter<IPAddress>(new ArgToIPConverter())
                .RegisterResource<IProfileApplier>(new ProfileApplier())
                .RegisterResource<IUserSettings>(new DefaultUserSettings());

            _handler.Handle(args);
        }

    }
}
