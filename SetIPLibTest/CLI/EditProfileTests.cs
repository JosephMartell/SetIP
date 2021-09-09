using FluentAssertions;
using SetIPCLI;
using SetIPLib;
using System.Linq;
using System.Net;
using Xunit;

namespace SetIPLibTest.CLI
{
    public class EditProfileTests
    {

        [Fact]
        public void Edit_ReplacesProfile_WithUpdatedVersion()
        {
            IProfileStore profileStore = new MemProfileStore();
            var editor = new EditProfile(profileStore)
            {
                IP = IPAddress.Parse("192.168.47.99"),
                Subnet = IPAddress.Parse("255.0.0.0")
            };
            editor.Edit("static 1");
            profileStore.Retrieve().Where(p => p.Name == "static 1").First().IP.Should().Be(IPAddress.Parse("192.168.47.99"));
            profileStore.Retrieve().Where(p => p.Name == "static 1").First().Subnet.Should().Be(IPAddress.Parse("255.0.0.0"));
        }
    }
}
