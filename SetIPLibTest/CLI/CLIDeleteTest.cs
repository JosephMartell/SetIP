using System.IO;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SetIPLib;
using SetIPCLI;
using System.Collections.Generic;
using System.Linq;

namespace SetIPLibTest.CLI
{
    [TestClass]
    public class CLIDeleteTest
    {
        [TestMethod]
        public void Deleted_profile_removed_from_store()
        {
            IProfileStore mps = new MemoryProfileStore();
            string profileNameToRemove = "Test Profile 2";

            List<Profile> profiles = new List<Profile>();
            for (int i = 0; i < 4; i++)
            {
                Profile p = new Profile(
                    string.Format("Test Profile {0}", i));
                profiles.Add(p);
            }
            mps.Store(profiles);
            ArgumentGroup ag = new ArgumentGroup(new string[] { "-d", profileNameToRemove });

            CLIDeleteProfile deleteCmd = new CLIDeleteProfile(ag);
            deleteCmd.Execute(ref mps);

            var deletedProfile = from p in mps.Retrieve()
                                 where p.Name == profileNameToRemove
                                 select p;
            Assert.IsTrue(deletedProfile.Count() == 0);
        }
    }
}
