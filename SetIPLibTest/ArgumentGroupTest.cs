using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SetIPCLI;
using System.Linq;
using System.Collections.Generic;

namespace SetIPLibTest
{
    [TestClass]
    public class ArgumentGroupTest
    {
        [TestMethod]
        public void Parsing_empty_argument_list_returns_empty_collection()
        {
            IEnumerable<ArgumentGroup> argGroups = ArgumentGroup.ParseArguments(new string[0]);
            Assert.AreEqual(0, argGroups.Count());
        }

        [TestMethod]
        public void Parsing_multiple_commands_with_no_arguments()
        {
            IEnumerable<ArgumentGroup> argGroups = ArgumentGroup.ParseArguments(new string[] { "-a", "-b", "-c" });
            Assert.AreEqual("-a", argGroups.ElementAt(0).Command);
            Assert.AreEqual("-b", argGroups.ElementAt(1).Command);
            Assert.AreEqual("-c", argGroups.ElementAt(2).Command);
        }

        [TestMethod]
        public void Arguments_with_double_dashes_are_accepted()
        {
            IEnumerable<ArgumentGroup> argGroups = ArgumentGroup.ParseArguments(new string[] { "--a", "--b", "--c" });
            Assert.AreEqual("-a", argGroups.ElementAt(0).Command);
            Assert.AreEqual("-b", argGroups.ElementAt(1).Command);
            Assert.AreEqual("-c", argGroups.ElementAt(2).Command);
        }

        [TestMethod]
        public void Arguments_with_slash_are_accepted()
        {
            IEnumerable<ArgumentGroup> argGroups = ArgumentGroup.ParseArguments(new string[] { "/a", "/b", "/c" });
            Assert.AreEqual("-a", argGroups.ElementAt(0).Command);
            Assert.AreEqual("-b", argGroups.ElementAt(1).Command);
            Assert.AreEqual("-c", argGroups.ElementAt(2).Command);

        }
    }
}
