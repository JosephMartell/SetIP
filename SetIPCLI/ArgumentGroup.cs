using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SetIPCLI {
    class ArgumentGroup {
        public string Command { get; }

        public IEnumerable<string> Arguments { get; }

        public ArgumentGroup(string command, IEnumerable<string> arguments) {
            Command = command.Replace("-", "").Replace("/", "");
            Arguments = arguments;
        }

        public ArgumentGroup(IEnumerable<string> commandWithArguments) {
            Command = commandWithArguments.First();
            Arguments = commandWithArguments.Skip(1);
        }
    }
}
