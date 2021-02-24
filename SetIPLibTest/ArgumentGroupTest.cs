using System;
using SetIPCLI;
using System.Linq;
using System.Collections.Generic;
using Xunit;
using FluentAssertions;

namespace SetIPLibTest
{
    public class ArgumentGroupTest
    {
        [Fact]
        public void Parsing_empty_argument_list_returns_empty_collection()
        {
            IEnumerable<ArgumentGroup> argGroups = ArgumentGroup.ParseArguments(new string[0]);
            argGroups.Should().HaveCount(0);
        }

        [Fact]
        public void Parsing_multiple_commands_with_no_arguments()
        {
            IEnumerable<ArgumentGroup> argGroups = ArgumentGroup.ParseArguments(new string[] { "-a", "-b", "-c" });
            argGroups.ElementAt(0).Command.Should().BeEquivalentTo("-a");
            argGroups.ElementAt(1).Command.Should().BeEquivalentTo("-b");
            argGroups.ElementAt(2).Command.Should().BeEquivalentTo("-c");
        }

        [Fact]
        public void Arguments_with_double_dashes_are_accepted()
        {
            IEnumerable<ArgumentGroup> argGroups = ArgumentGroup.ParseArguments(new string[] { "--a", "--b", "--c" });
            argGroups.ElementAt(0).Command.Should().BeEquivalentTo("-a");
            argGroups.ElementAt(1).Command.Should().BeEquivalentTo("-b");
            argGroups.ElementAt(2).Command.Should().BeEquivalentTo("-c");
        }

        [Fact]
        public void Arguments_with_slash_are_accepted()
        {
            IEnumerable<ArgumentGroup> argGroups = ArgumentGroup.ParseArguments(new string[] { "/a", "/b", "/c" });
            argGroups.ElementAt(0).Command.Should().BeEquivalentTo("-a");
            argGroups.ElementAt(1).Command.Should().BeEquivalentTo("-b");
            argGroups.ElementAt(2).Command.Should().BeEquivalentTo("-c");

        }
    }
}
