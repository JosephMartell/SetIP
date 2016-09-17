using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SetIPLib;

namespace SetIPCLI {
    interface ICLICommand {
        ArgumentGroup Arguments{ get; }
        void Execute(ref IProfileStore store);
    }
}
