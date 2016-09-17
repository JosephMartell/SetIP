using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SetIPLib;

namespace SetIPCLI {
    class CLIUnknown : ICLICommand {
        public ArgumentGroup Arguments { get; }
        public CLIUnknown(ArgumentGroup args) {
            Arguments = args;
        }

        public void Execute(ref IProfileStore store) {
            throw new UnknownCommandException(Arguments.Command);
        }
    }


}
