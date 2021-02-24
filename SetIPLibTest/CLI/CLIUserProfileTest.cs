//using System;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using SetIPLib;
//using SetIPCLI;

//namespace SetIPLibTest.CLI
//{
//    [TestClass]
//    public class CLIUserProfileTest
//    {
//        private class MockProfileApplier : IProfileApplier
//        {
//            public MockProfileApplier()
//            {
//                InterfaceAppliedTo = string.Empty;
//                AppliedProfile = null;
//            }
//            public void Apply(Profile profile, string interfaceName)
//            {
//                AppliedProfile = profile;
//                InterfaceAppliedTo = interfaceName;
//            }

//            public string InterfaceAppliedTo { get; private set; }
//            public Profile AppliedProfile { get; private set; }
//        }


//        [TestMethod]
//        public void Execute_pases_supplied_interface_name_to_applier()
//        {
//            MockProfileApplier mock = new MockProfileApplier();
//            ArgumentGroup args = new ArgumentGroup(new string[] { "-u", "test", "test interface" });

//            CLIUseProfile use = new CLIUseProfile(args, mock, new UserSettings());
//            IProfileStore mps = new MemoryProfileStore();

//            use.Execute(ref mps);
//            Assert.AreEqual("test interface", mock.InterfaceAppliedTo);
//        }
//    }
//}
