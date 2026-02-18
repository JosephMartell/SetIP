using CLImber;
using SetIPLib;
using System;
using System.IO;
using System.Linq;

namespace SetIPCLI
{
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
