using FluentAssertions;
using SetIPCLI;
using SetIPLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Xunit;

namespace SetIPLibTest.CLI
{
    public class MemProfileStore
        : IProfileStore
    {
        public IProfileEncoder Encoder => throw new NotImplementedException();

        public List<Profile> Profiles { get; private set; } = new List<Profile>()
        {
            Profile.CreateDHCPProfile("profile 1"),
            Profile.CreateStaticProfile("static 1",
                IPAddress.Parse("192.168.1.99"),
                IPAddress.Parse("255.255.255.0")),
            Profile.CreateStaticProfile("static 2",
                IPAddress.Parse("172.16.99.99"),
                IPAddress.Parse("255.255.0.0"),
                IPAddress.Parse("172.16.0.1"))
        };

        public IEnumerable<Profile> Retrieve()
        {
            return Profiles;
        }

        public void Store(IEnumerable<Profile> profiles)
        {
            Profiles = profiles.ToList();
        }
    }

    public class AddProfileTests
    {
        private static readonly Profile testStaticProfile1 = Profile.CreateStaticProfile("test static profile 1", IPAddress.Parse("10.10.1.1"), IPAddress.Parse("255.0.0.0"));

        [Fact]
        public void Add_command_adds_dynamic_profile()
        {
            IProfileStore profileStore = new MemProfileStore();
            var _adder = new AddProfile(profileStore);
            int startingCount = profileStore.Retrieve().Count();
            string newProfileName = "add command adds dynamic profile";
            _adder.AddDynamicProfile(newProfileName);

            profileStore.Retrieve().Where(p => p.Name == newProfileName).Should().HaveCount(1);
        }

        [Fact]
        public void Add_command_adds_static_profile_with_ip_and_subnet()
        {
            IProfileStore profileStore = new MemProfileStore();
            var _adder = new AddProfile(profileStore);
            _adder.AddStaticProfile(testStaticProfile1.Name, testStaticProfile1.IP, testStaticProfile1.Subnet);
            profileStore.Retrieve().Where(p => p.Name == testStaticProfile1.Name).Should().HaveCount(1);
            profileStore.Retrieve().Where(p => p.Name == testStaticProfile1.Name).First().IP.Should().BeEquivalentTo(testStaticProfile1.IP);
            profileStore.Retrieve().Where(p => p.Name == testStaticProfile1.Name).First().Subnet.Should().BeEquivalentTo(testStaticProfile1.Subnet);
        }
    }
}
