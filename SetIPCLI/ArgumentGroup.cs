using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Text;
using System.Threading.Tasks;

namespace SetIPCLI {
    /// <summary>
    /// Stores a CLI command and arguments for that command.
    /// </summary>
    internal class ArgumentGroup {

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
            Command = commandWithArguments.FirstOrDefault();
            Arguments = commandWithArguments?.Skip(1);
        }

        public static ArgumentGroup EmptyGroup {
            get {
                return new ArgumentGroup(new string[0]);
            }
        }

        private static string[] ScrubArgs(IEnumerable<string> args)
        {
            var argList = args.ToArray();
            for (int i = 0; i < argList.Length; i++)
            {
                if (argList[i].StartsWith("--"))
                {
                    argList[i] = argList[i].Remove(0, 1);
                }
                if (argList[i].StartsWith("/"))
                {
                    argList[i] = "-" + argList[i].Remove(0, 1);
                }
            }

            return argList;
        }


        public static IEnumerable<ArgumentGroup> ParseArguments(IEnumerable<string> argumentList)
        {
            argumentList = ScrubArgs(argumentList);
            if (argumentList.Any(s => s.StartsWith("-")))
            {
                List<ArgumentGroup> argGroups = new List<SetIPCLI.ArgumentGroup>();
                List<string> argGroup = null;
                foreach (var a in argumentList)
                {
                    if (a.StartsWith("-"))
                    {
                        if (argGroup != null)
                        {
                            argGroups.Add(new ArgumentGroup(argGroup));
                        }
                        argGroup = new List<string>();
                    }
                    argGroup.Add(a);
                }
                argGroups.Add(new ArgumentGroup(argGroup));

                return argGroups;
            }
            else
                return new List<ArgumentGroup>();
        }
    }
}
