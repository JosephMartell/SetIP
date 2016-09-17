using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SetIPCLI {

    /// <summary>
    /// Parses Argument Groups and returns an isntance of the appropriate command class.
    /// </summary>
    static class CLICommandFactory {


        /// <summary>
        /// Returns an collection of CLI Commands based on a collection of argument groups passed to it.  Depends on the GetCommand method
        /// </summary>
        /// <param name="arguments">Parsed command line arguments</param>
        /// <returns>Collection of executable commands.</returns>
        public static IEnumerable<ICLICommand> GetCommands(IEnumerable<ArgumentGroup> arguments) {
            List<ICLICommand> commands = new List<ICLICommand>();

            foreach (var arg in arguments) {
                commands.Add(GetCommand(arg));
            }
            return commands;
        }


        /// <summary>
        /// Returns a single, executable CLI Command.  Any unknown commands are rturned as CLIUnknown.
        /// </summary>
        /// <param name="arg"></param>
        /// <returns>A single, executable CLI Command object.</returns>
        public static ICLICommand GetCommand(ArgumentGroup arg) {
            ICLICommand returnCommand;
            switch (arg.Command.ToUpper()) {
                case "-A":
                case "-ADD":
                    returnCommand = new AddProfile(arg);
                    break;
                default:
                    returnCommand = new CLIUnknown(arg);
                    break;
            }
            return returnCommand;
        }
    }
}
