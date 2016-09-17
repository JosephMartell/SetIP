using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SetIPCLI {
    interface ICLICommand {
        CLICommandPriority Priority { get; }
        ArgumentGroup Arguments{ get; }
        void Execute();
    }
}
