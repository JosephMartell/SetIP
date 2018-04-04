using Microsoft.VisualStudio.TestTools.UnitTesting;
using SetIPLib;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;

namespace SetIPLibTest
{
    [TestClass]
    public class XMLProfileEncoderTest
    {
        //string xmlExample = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\" ?><Profiles><profile name=\"test profile\" useDHCP=\"true\" /></Profiles>";
        string badXML = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\" ?><Profiles><profile name=\"test profile\" useDHCP=\"true\" /></Profiles123>";
        Profile examplelProfile = Profile.CreateDHCPProfile("test profile");
        MemoryStream storageStream;
        StreamProfileStore profileStore;

        [TestMethod]
        public void Decode_zero_byte_array_returns_empty_enumerable()
        {
            byte[] emptyFileContents = new byte[0];
            XMLProfileEncoder xmlpe = new XMLProfileEncoder();
            IEnumerable<Profile> decodedProfiles = xmlpe.Decode(emptyFileContents);
            Assert.AreEqual(0, decodedProfiles.Count());
        }


        [TestMethod]
        public void Malformed_xml_doc_throws_exception()
        {
            storageStream = new MemoryStream();
            PopulateMemoryStream(storageStream, badXML);
            profileStore = new StreamProfileStore(storageStream, new XMLProfileEncoder());
            XMLProfileEncoder xmlpe = new XMLProfileEncoder();
            Assert.ThrowsException<XmlException>(() => xmlpe.Decode(storageStream.GetBuffer()));
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

        public void PopulateMemoryStream(MemoryStream ms, string xml)
        {
            ms.Write(
                UTF8Encoding.UTF8.GetBytes(xml),
                0,
                UTF8Encoding.UTF8.GetBytes(xml).Count());
        }
    }
}
