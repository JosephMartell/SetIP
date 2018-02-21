using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SetIPCLI;
using SetIPLib;
using System.IO;

namespace SetIPLibTest.CLI
{
    [TestClass]
    public class CLIChangeSettingTest
    {
        IProfileStore mps = new MemoryProfileStore();
        StringWriter redirectedOutput;

        [TestInitialize]
        public void Setup()
        {
            redirectedOutput = new StringWriter();
            Console.SetOut(redirectedOutput);
        }

        [TestMethod]
        public void Retrieve_current_directory_for_profiles()
        {
            ArgumentGroup ag = new ArgumentGroup(new string[] { "-s", "ProfileFileLocation" });
            CLIChangeSetting cs = new CLIChangeSetting(ag);
            cs.Execute(ref mps);

            var output = redirectedOutput.ToString();
            var expectedPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SetIP\\profiles.xml");
            //Assert.AreEqual(@"C:\Users\jbmartell\AppData\Roaming\SetIP\profiles.xml", output.Trim());
            Assert.AreEqual(expectedPath, output.Trim());
        }

        [TestMethod]
        public void Set_custom_profile_directory()
        {
            //send command to save new path
            var newPath = @"C:\Users\jbmartell\Documents\profiles.xml";
            ArgumentGroup setNewPath = new ArgumentGroup(new string[] { "-s", "ProfileFileLocation", newPath });
            ICLICommand csNewPath = new CLIChangeSetting(setNewPath);
            csNewPath.Execute(ref mps);

            //read current setting 
            ArgumentGroup ag = new ArgumentGroup(new string[] { "-s", "ProfileFileLocation"});
            CLIChangeSetting cs = new CLIChangeSetting(ag);
            cs.Execute(ref mps);

            //verify new setting was stored
            var output = redirectedOutput.ToString();
            var expectedPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SetIP\\profiles.xml");
            Assert.AreEqual(newPath, output.Trim());
        }
    }
}
