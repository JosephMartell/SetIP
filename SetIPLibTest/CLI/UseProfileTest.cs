using SetIPLib;
using SetIPCLI;
using Xunit;
using FluentAssertions;
using System.Linq;

namespace SetIPLibTest.CLI
{
    public class UseProfileTest
    {
        public class ApplierMock
            : IProfileApplier
        {
            public Profile LastAppliedProfile
            {
                get;
                private set;
            } = Profile.DHCPDefault;

            public string LastAppliedInterface
            {
                get;
                private set;
            }

            public void Apply(Profile profile, string interfaceName)
            {
                LastAppliedProfile = profile;
                LastAppliedInterface = interfaceName;
            }
        }

        [Fact]
        public void Default_command_applies_DHCP()
        {
            var memStore = new MemProfileStore();
            var applier = new ApplierMock();
            var settings = new FakeSettings();

            UseProfile use = new UseProfile(memStore, applier, settings);
            use.UseDefaultDHCP();

            applier.LastAppliedProfile.Should().BeEquivalentTo(Profile.DHCPDefault);
            applier.LastAppliedInterface.Should().BeEquivalentTo(settings.DefaultNIC);
        }

        [Fact]
        public void Selected_profile_is_applied_to_default_nic()
        {
            var memStore = new MemProfileStore();
            var applier = new ApplierMock();
            var settings = new FakeSettings();

            UseProfile use = new UseProfile(memStore, applier, settings);

            var rng = new System.Random();
            var selectedProfile = memStore.Retrieve().ToList().OrderBy(p => rng.Next()).First();

            use.UseProfileByName(selectedProfile.Name);

            applier.LastAppliedProfile.Should().BeEquivalentTo(selectedProfile);
            applier.LastAppliedInterface.Should().BeEquivalentTo(settings.DefaultNIC);
        }

        [Fact]
        public void Selected_profile_is_applied_to_specific_nic()
        {
            var memStore = new MemProfileStore();
            var applier = new ApplierMock();
            var settings = new FakeSettings();

            UseProfile use = new UseProfile(memStore, applier, settings);

            var rng = new System.Random();
            var selectedProfile = memStore.Retrieve().ToList().OrderBy(p => rng.Next()).First();

            var nicName = "nic name " + rng.Next();
            use.UseProfileOnNIC(selectedProfile.Name, nicName);

            applier.LastAppliedProfile.Should().BeEquivalentTo(selectedProfile);
            applier.LastAppliedInterface.Should().BeEquivalentTo(nicName);
        }
    }
}
