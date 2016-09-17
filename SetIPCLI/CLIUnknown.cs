using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SetIPCLI {
    class CLIUnknown : ICLICommand {
        public CLICommandPriority Priority { get; } = CLICommandPriority.Low;
        public ArgumentGroup Arguments { get; }
        public CLIUnknown(ArgumentGroup args) {
            Arguments = args;
        }

        public void Execute() {
            throw new UnknownCommandException(Arguments.Command);
        }
    }


}
