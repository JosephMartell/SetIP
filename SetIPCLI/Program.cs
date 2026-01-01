using CLImber;
using SetIPLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

    [CommandClass("import", ShortDescription = "Import profiles from an external file.")]
    internal class ImportProfiles
    {
        private readonly IProfileStore profileStore;

        [CommandOption("overwrite", Abbreviation = 'o', Description = "Overwrite the local profile if the imported file contains a profile with the same name.")]
        public bool Overwrite { get; set; }

        public ImportProfiles(IProfileStore profileStore)
        {
            this.profileStore = profileStore;
        }

        [CommandHandler(ShortDescription = "Imports all profiles from an external file.")]
        public void Import(string filePath)
        {
            IProfileStore importStore = new StreamProfileStore(
                new FileStream(filePath, FileMode.Open),
                new XMLProfileEncoder());

            var importProfiles = importStore.Retrieve();

            var localProfiles = profileStore.Retrieve();

            foreach (var importProfile in importProfiles)
            {
                if (!Overwrite && (localProfiles.Where(p => p.Name.ToUpper() == importProfile.Name.ToUpper()).Count() > 0))
                {
                    Console.WriteLine($"Profile {importProfile.Name} already exists. Skipping import...");
                    continue;
                }

                //if we made it here then the profile to import either doesn't exist OR we are overwriting on import. This should handle both cases.
                localProfiles = localProfiles.Where(p => p.Name.ToUpper() != importProfile.Name.ToUpper()).Append(importProfile);
            }

            profileStore.Store(localProfiles);
        }

        [CommandHandler(ShortDescription = "Specifies a specific profile to import from an external file.")]
        public void Import(string filePath, string profileName)
        {
            IProfileStore importStore = new StreamProfileStore(
                new FileStream(filePath, FileMode.Open),
                new XMLProfileEncoder());

            //This should work when the method correctly accepts any number of profile names. For now, you have to import 1 by 1.
            //var filterNamesList = profileNames.Select(n => n.ToUpper());

            var importProfiles = importStore.Retrieve().Where(p => p.Name.ToUpper() == profileName.ToUpper());

            var localProfiles = profileStore.Retrieve();

            foreach (var importProfile in importProfiles)
            {
                if (!Overwrite && (localProfiles.Where(p => p.Name.ToUpper() == importProfile.Name.ToUpper()).Count() > 0))
                {
                    Console.WriteLine($"Profile {importProfile.Name} already exists. Skipping import...");
                    continue;
                }

                //if we made it here then the profile to import either doesn't exist OR we are overwriting on import. This should handle both cases.
                localProfiles = localProfiles.Where(p => p.Name.ToUpper() != importProfile.Name.ToUpper()).Append(importProfile);
            }
            profileStore.Store(localProfiles);

        }
    }
}
