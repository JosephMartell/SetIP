using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SetIPLib;

namespace SetIPCLI {
    class CLIListCommands : ICLICommand {
        public ArgumentGroup Arguments { get; }

        public IEnumerable<string> CommandSummary() {
            return new string[]
            {
                string.Format(
                    "{0, -15} {1, -64}",
                    "List Commands",
                    "-?"
                    )
            };
        }

        public CLIListCommands(ArgumentGroup arg) {

        }

        public void Execute(ref IProfileStore store) {
            new CLIAddProfile(ArgumentGroup.EmptyGroup).CommandSummary().ToList().ForEach(Console.WriteLine);
            new CLIEditProfile(ArgumentGroup.EmptyGroup).CommandSummary().ToList().ForEach(Console.WriteLine);
            new CLIListProfiles(ArgumentGroup.EmptyGroup).CommandSummary().ToList().ForEach(Console.WriteLine);
            new CLIUseProfile(ArgumentGroup.EmptyGroup).CommandSummary().ToList().ForEach(Console.WriteLine);
        }

        public string Help() {
            return "Usage: setipcli -?\n" +
                   " - provides a listing of all available commands and examplel usages.";
        }
    }
}
