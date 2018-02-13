using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SetIPCLI;
using System.Collections.Generic;
using System.Linq;

namespace SetIPLibTest.CLI
{
    [TestClass]
    public class CLICommandFactoryTest
    {
        [TestMethod]
        public void Get_Commands_returns_default_Command_when_no_args_are_passed()
        {
            var listCommands = new CLIListCommands(ArgumentGroup.EmptyGroup);
            var cliCommands = CLICommandFactory.GetCommands(new List<ArgumentGroup>(), listCommands);
            Assert.AreEqual(1, cliCommands.Count());
            Assert.AreEqual(listCommands.Help(), cliCommands.ElementAt(0).Help());
        }
    }
}
