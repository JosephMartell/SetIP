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
        [TestClass]
        public class UnrecognizedSettings
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
            public void Passing_no_name_does_not_throw_exception()
            {
                ArgumentGroup ag = new ArgumentGroup(new string[] { "-s" });
                CLIChangeSetting cs = new CLIChangeSetting(ag);
                cs.Execute(ref mps);
                var response = redirectedOutput.ToString();
            }

            [TestMethod]
            public void Unrecognized_name_prints_warning()
            {
                ArgumentGroup ag = new ArgumentGroup(new string[] { "-s", "UnknownSetting" });
                CLIChangeSetting cs = new CLIChangeSetting(ag);
                cs.Execute(ref mps);
                var response = redirectedOutput.ToString();

                Assert.AreEqual("There is no setting named \"UnknownSetting\". For a list of valid setting names enter -s with no further commands.", response.Trim());
            }

            [TestMethod]
            public void Unrecognized_name_with_argument_prints_warning()
            {
                ArgumentGroup ag = new ArgumentGroup(new string[] { "-s", "UnknownSetting", "Garbage Setting" });
                CLIChangeSetting cs = new CLIChangeSetting(ag);
                cs.Execute(ref mps);
                var response = redirectedOutput.ToString();

                Assert.AreEqual("There is no setting named \"UnknownSetting\". For a list of valid setting names enter -s with no further commands.", response.Trim());
            }
        }

        [TestClass]
        public class ProfilePathSetting
        {
            IProfileStore mps = new MemoryProfileStore();
            StringWriter redirectedOutput;

            [TestInitialize]
            public void Setup()
            {
                redirectedOutput = new StringWriter();
                Console.SetOut(redirectedOutput);
            }

            [TestCleanup]
            public void Cleanup()
            {
                var originalPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SetIP\\profiles.xml");
                ArgumentGroup setNewPath = new ArgumentGroup(new string[] { "-s", "ProfileFileLocation", originalPath });
                ICLICommand csNewPath = new CLIChangeSetting(setNewPath);
                csNewPath.Execute(ref mps);

            }

            [TestMethod]
            public void Retrieve_current_directory_for_profiles()
            {
                ArgumentGroup ag = new ArgumentGroup(new string[] { "-s", "ProfileFileLocation" });
                CLIChangeSetting cs = new CLIChangeSetting(ag);
                cs.Execute(ref mps);

                var output = redirectedOutput.ToString();
                var expectedPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SetIP\\profiles.xml");
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
                ArgumentGroup ag = new ArgumentGroup(new string[] { "-s", "ProfileFileLocation" });
                CLIChangeSetting cs = new CLIChangeSetting(ag);
                cs.Execute(ref mps);

                //verify new setting was stored
                var output = redirectedOutput.ToString();
                var expectedPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SetIP\\profiles.xml");
                Assert.AreEqual(newPath, output.Trim());
            }

        }

        [TestClass]
        public class DefaultNICSetting
        {
            IProfileStore mps = new MemoryProfileStore();
            StringWriter redirectedOutput;

            [TestInitialize]
            public void Setup()
            {
                redirectedOutput = new StringWriter();
                Console.SetOut(redirectedOutput);
            }

            [TestCleanup]
            public void Cleanup()
            {
                var originalNIC = "Local Area Connection";
                ArgumentGroup setNewNIC = new ArgumentGroup(new string[] { "-s", "DefaultNIC", originalNIC });
                ICLICommand csNewNIC = new CLIChangeSetting(setNewNIC);
                csNewNIC.Execute(ref mps);
            }

            [TestMethod]
            public void Retrieve_current_NIC_name()
            {
                ArgumentGroup ag = new ArgumentGroup(new string[] { "-s", "DefaultNIC" });
                CLIChangeSetting cs = new CLIChangeSetting(ag);
                cs.Execute(ref mps);

                var output = redirectedOutput.ToString();
                var expectedName = "Local Area Connection";
                Assert.AreEqual(expectedName, output.Trim());
            }

            [TestMethod]
            public void New_default_name_is_stored_correctly()
            {
                //send command to save new NIC Name
                var newNIC = @"Ethernet"; //used in windows 10
                ArgumentGroup setNewNIC = new ArgumentGroup(new string[] { "-s", "DefaultNIC", newNIC });
                ICLICommand csNewNIC = new CLIChangeSetting(setNewNIC);
                csNewNIC.Execute(ref mps);

                //read current setting 
                ArgumentGroup ag = new ArgumentGroup(new string[] { "-s", "DefaultNIC" });
                CLIChangeSetting cs = new CLIChangeSetting(ag);
                cs.Execute(ref mps);

                //verify new setting was stored
                var output = redirectedOutput.ToString();
                Assert.AreEqual(newNIC, output.Trim());

            }
        }
    }
}
