using CLImber;
using SetIPLib;
using System;
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

            _handler.Handle(args);
        }

    }
}
