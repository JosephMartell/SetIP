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

//    internal class CLIDeleteProfile :
//        ICLICommand
//    {
//        public ArgumentGroup Arguments { get; }

//        public CLIDeleteProfile(ArgumentGroup args)
//        {
//            Arguments = args;
//        }

//        public IEnumerable<string> CommandSummary()
//        {
//            string format = "{0, -15} {1, -63}";
//            List<string> summary = new List<string>();
//            Action<string, string> summaryFormat = 
//                (s1, s2) => summary.Add(string.Format(format, s1, s2));

//            summaryFormat(
//                "Delete Profile",
//                "-d \"Profile Name\"");
//            return summary;
//        }

//        public void Execute(ref IProfileStore store)
//        {
//            //must be called with 1 argument only
//            string targetProfileName = Arguments.Arguments.DefaultIfEmpty(string.Empty).First();

//            if (targetProfileName != string.Empty)
//            {
//                var currentProfiles = store?.Retrieve();
//                currentProfiles = from p in currentProfiles
//                                  where p.Name != targetProfileName
//                                  select p;

//                store.Store(currentProfiles);
//            }
//        }

//        public string Help()
//        {
//            return "Usage: setipcli -d \"Profile Name\" \n" +
//                " - profile will be deleted immediately.  This action cannot be undone.";

//        }
//    }
//}
