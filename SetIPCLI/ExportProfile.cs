using CLImber;
using SetIPLib;
using System;
using System.IO;
using System.Linq;

namespace SetIPCLI
{
    [CommandClass("export", ShortDescription = "Export a profile to an xml file. Will not export if a profile with the same name already exists in the file.")]
    internal class ExportProfile
    {
        private readonly IProfileStore profileStore;

        [CommandOption("overwrite", Abbreviation = 'o', Description = "If set, the export will overwrite the destination file if it already exists. Default behavior is to append.")]
        public bool Overwrite { get; set; }

        public ExportProfile(IProfileStore profileStore)
        {
            this.profileStore = profileStore;
        }

        [CommandHandler(ShortDescription = "Export the specified profile to the designated file. The file will be created if it does not exist. ")]
        public void Export(string profileName, string filePath)
        {
            var profiles = profileStore.Retrieve().Where(p => p.Name.ToUpper() == profileName.ToUpper());

            if (profiles.Count() <= 0)
            {
                Console.WriteLine($"Profile '{profileName}' not found.");
                return;
            }

            var profile = profiles.First();

            IProfileEncoder profileEncoder = new XMLProfileEncoder();

            FileStream fs;
            if (Overwrite)
            {
                fs = new FileStream(filePath, FileMode.Create);
            }
            else
            {
                fs = new FileStream(filePath, FileMode.OpenOrCreate);
            }
            IProfileStore exportStore = new StreamProfileStore(fs, profileEncoder);
            var targetProfiles = exportStore.Retrieve();

            if (targetProfiles.Where(p => p.Name.ToUpper() == profileName.ToUpper()).Count() > 0)
            {
                Console.WriteLine($"Export file already contains a profile named {profileName}");
                return;
            }

            exportStore.Store(targetProfiles.Append(profile));
        }
    }
}
