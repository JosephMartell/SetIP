using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CLImber;
using SetIPLib;

namespace SetIPCLI
{
    [CommandClass("delete")]
    public class DeleteProfile
    {
        public DeleteProfile(IProfileStore store)
        {
            Store = store;
        }

        public IProfileStore Store { get; }

        [CommandHandler]
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
