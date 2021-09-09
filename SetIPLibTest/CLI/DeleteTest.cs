using FluentAssertions;
using SetIPCLI;
using System.Linq;

namespace SetIPLibTest.CLI
{

    public class DeleteProfileTests
    {
        public void Delete_cmd_deletes_selected_profile()
        {
            var profileStore = new MemProfileStore();
            DeleteProfile del = new DeleteProfile(profileStore);
            var existingProfile = profileStore.Retrieve().First();
            del.DeleteProfileByName(existingProfile.Name);
            profileStore.Retrieve().Should().NotContain(existingProfile);
        }
    }

}