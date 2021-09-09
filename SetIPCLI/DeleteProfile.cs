using CLImber;
using SetIPLib;
using System.Linq;

namespace SetIPCLI
{
    [CommandClass("delete", ShortDescription = "Deletes a profile from the storage file.")]
    public class DeleteProfile
    {
        public DeleteProfile(IProfileStore store)
        {
            Store = store;
        }

        public IProfileStore Store { get; }

        [CommandHandler(ShortDescription = "Delets a profile with the provided name.")]
        public void DeleteProfileByName(string profileName)
        {
            if (profileName != string.Empty)
            {
                var currentProfiles = Store.Retrieve();
                currentProfiles = from p in currentProfiles
                                  where p.Name.ToUpper() != profileName.ToUpper()
                                  select p;

                Store.Store(currentProfiles);
            }
        }
    }
}
