using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SetIPLib;
using System.Linq;
using System.Collections.Generic;
using System.Xml;
using System.Net;

namespace SetIPLibTest
{
    [TestClass]
    public class XMLProfileEncoderTest
    {
        [TestMethod]
        public void Decode_zero_byte_array_throws_exception()
        {
            byte[] emptyFileContents = new byte[0];
            XMLProfileEncoder xmlpe = new XMLProfileEncoder();
            IEnumerable<Profile> decodedProfiles = null;
            Assert.ThrowsException<XmlException>(() => decodedProfiles = xmlpe.Decode(emptyFileContents));
        }

        [TestMethod]
        public void Encoded_profile_decodes_identically()
        {
            Profile originalProfile = Profile.CreateStaticProfile("Test Static Profile 1",
                IPAddress.Parse("192.168.170.138"),
                IPAddress.Parse("255.255.255.250"),
                IPAddress.Parse("192.168.170.139"),
                new List<IPAddress>() {
                    IPAddress.Parse("10.11.12.13"),
                    IPAddress.Parse("12.23.24.45") });

            XMLProfileEncoder enc = new XMLProfileEncoder();

            Profile decodedProfile = enc.Decode(
                enc.Header.Concat(
                    enc.Encode(originalProfile))
                    .Concat(enc.Footer)
                    .ToArray()).First();

            Assert.AreEqual(originalProfile, decodedProfile);
        }
    }
}
