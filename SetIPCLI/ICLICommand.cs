using SetIPLib;
using System.Collections.Generic;

namespace SetIPCLI {
    interface ICLICommand {
        ArgumentGroup Arguments{ get; }
        void Execute(ref IProfileStore store);
        string Help();
        IEnumerable<string> CommandSummary();
    }
}
