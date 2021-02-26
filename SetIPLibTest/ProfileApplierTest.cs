//using System;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using SetIPLib;
//using System.Net;

//namespace SetIPLibTest
//{
//    [TestClass]
//    public class ProfileApplierTest
//    {
//        static Profile dhcpProfile = Profile.CreateDHCPProfile("test dhcp");
//        public ProfileApplierTest()
//        {

//        }

//        [TestMethod]
//        [DataRow("Local Area Connection")] // Windows XP, 7 Default
//        [DataRow("Ethernet")]  // Windows 10 Default
//        [DataRow("Weird Adapter")]
//        public void Interface_name_is_used_with_dhcp_args(string interfaceName)
//        {
//            var args = ProfileApplier.CreateDHCPNetshArgs(interfaceName);
//            Assert.AreEqual($"interface ip set address \"{interfaceName}\" dhcp", args);
//        }

//        [TestMethod]
//        [DataRow("Local Area Connection")] // Windows XP, 7 Default
//        [DataRow("Ethernet")]  // Windows 10 Default
//        [DataRow("Weird Adapter")]
//        public void Static_args_for_simple_profile(string interfaceName)
//        {
//            Profile simpleProfile = Profile.CreateStaticProfile("test static 1", IPAddress.Parse("192.168.1.1"), IPAddress.Parse("255.255.255.0"));
//            var args = ProfileApplier.CreateStaticNetshArgs(interfaceName, simpleProfile);
//            string expected = $"interface ip set address \"{interfaceName}\" static " +
//                $"{simpleProfile.IP.ToString()} {simpleProfile.Subnet.ToString()}";
//            Assert.AreEqual(expected, args);
//        }

//        [TestMethod]
//        [DataRow("Local Area Connection")] // Windows XP, 7 Default
//        [DataRow("Ethernet")]  // Windows 10 Default
//        [DataRow("Weird Adapter")]
//        public void Static_args_for_profile_with_gateway(string interfaceName)
//        {
//            Profile gatewayProfile = Profile.CreateStaticProfile("test static 1", IPAddress.Parse("192.168.1.1"), IPAddress.Parse("255.255.255.0"), IPAddress.Parse("192.168.1.254"));
//            var args = ProfileApplier.CreateStaticNetshArgs(interfaceName, gatewayProfile);
//            string expected = $"interface ip set address \"{interfaceName}\" static " +
//                $"{gatewayProfile.IP.ToString()} {gatewayProfile.Subnet.ToString()} {gatewayProfile.Gateway.ToString()}";
//            Assert.AreEqual(expected, args);
//        }

//    }
//}
