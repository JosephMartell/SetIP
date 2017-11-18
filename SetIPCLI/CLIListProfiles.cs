using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SetIPLib;

namespace SetIPCLI
{
    /// <summary>
    /// Returns a list of all profiles currently stored.
    /// Expected CLI syntax:
    ///   -l
    ///   -list
    /// </summary>
    class CLIListProfiles : ICLICommand {
        public ArgumentGroup Arguments { get; }

        public CLIListProfiles(ArgumentGroup args)
        {
            Arguments = args;

        }

        public void Execute(ref IProfileStore store) {
            IEnumerable<Profile> profiles;
            if (Arguments.Arguments.Count() > 0) {
                string filter = Arguments.Arguments.First();
                profiles = store.Retrieve().Where((p, b) => p.Name.ToUpper().Contains(filter.ToUpper()));
            }
            else {
                profiles = store.Retrieve();
            }

            foreach (var profile in profiles.OrderBy(p => p.Name)) {
                if (profile.UseDHCP) {
                    Console.WriteLine("{0,-35} DHCP", profile.Name);
                }
                else
                {
                    Console.WriteLine("{0,-30} {1,-15} {2,-15} {3,-15}", profile.Name, profile.IP.ToString(), profile.Subnet.ToString(), profile.Gateway.ToString());
                }
            }
        }

        public string Help()
        {
            return $"Usage: setipcli -l\n" +
                    " Returns a listing of all saved profiles";
        }

        private delegate void cmdSumFormat(string s1, string s2);
        public IEnumerable<string> CommandSummary() {

            string format = "{0, -15} {1, -63}";
            List<string> summary = new List<string>();
            cmdSumFormat addLine = (s1, s2) => summary.Add(string.Format(format, s1, s2));

            addLine(
                    "List Profiles",
                    "-l [filter]");
            addLine(
                "",
                "filter is any sequence of characters that will be");
            addLine(
                "",
                "matched against profile names.  Wildcard chcaracters");
            addLine(
                "",
                "are not supported at this time.");

            return summary;
        }

    }
}
