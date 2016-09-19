using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SetIPCLI {
    /// <summary>
    /// Stores a CLI command and arguments for that command.
    /// </summary>
    class ArgumentGroup {

        /// <summary>
        /// The CLI argument that indicates the start of a command.  Commands are delimited by a '-' character and should be the first argument passed to constructor.
        /// </summary>
        public string Command { get; }

        /// <summary>
        /// Arguments that the target command will use for processing.
        /// </summary>
        public IEnumerable<string> Arguments { get; }

        /// <summary>
        /// This constructor will parse out the first item as the command and store the rest of the items as arguments.
        /// </summary>
        /// <param name="commandWithArguments">A single list containing the command flag as the first element ('-a' or '-e', etc) and all other necessary arguments for that command.</param>
        public ArgumentGroup(IEnumerable<string> commandWithArguments) {
            Command = commandWithArguments.First();
            Arguments = commandWithArguments.Skip(1);
        }
    }
}
