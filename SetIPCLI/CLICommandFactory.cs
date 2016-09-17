using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SetIPCLI {
    static class CLICommandFactory {

        public static IEnumerable<ICLICommand> GetCommands(IEnumerable<ArgumentGroup> arguments) {
            List<ICLICommand> commands = new List<ICLICommand>();

            foreach (var arg in arguments) {
                commands.Add(GetCommand(arg));
            }
            return commands;
        }

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
