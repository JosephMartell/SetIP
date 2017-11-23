using System.IO;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SetIPLib;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace SetIPLibTest
{
    [TestClass]
    public class StreamProfileStoreTest
    {
        string xmlExample = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\" ?><Profiles><profile name=\"test profile\" useDHCP=\"true\" /></Profiles>";
        Profile examplelProfile = Profile.CreateDHCPProfile("test profile");

        [TestMethod]
        public void Decodes_example_profile_correctly()
        {
            MemoryStream ms = new MemoryStream();
            ms.Write(UTF8Encoding.UTF8.GetBytes(xmlExample), 0, UTF8Encoding.UTF8.GetBytes(xmlExample).Count());

            StreamProfileStore sps = new StreamProfileStore(ms, new XMLProfileEncoder());
            var profiles = sps.Retrieve();
            var target = from p in profiles
                         where p.Name == "test profile"
                         select p;

            Assert.IsTrue(target.Count() == 1);
            var retrievedProfile = target.First();
            Assert.IsTrue(retrievedProfile.Name == "test profile");
            Assert.IsTrue(retrievedProfile.UseDHCP);
            Assert.AreEqual(examplelProfile, retrievedProfile);
        }

        [TestMethod]
        public void Encodes_example_profile_correctly_as_XML()
        {
            MemoryStream ms = new MemoryStream();
            StreamProfileStore sps = new StreamProfileStore(ms, new XMLProfileEncoder());

            List<Profile> profiles = new List<Profile>();
            profiles.Add(examplelProfile);
            sps.Store(profiles);
            string retrievedXML = UTF8Encoding.UTF8.GetString(ms.GetBuffer()).Trim().Replace("\0", "");
            Assert.AreEqual(xmlExample, retrievedXML);
        }

        [TestMethod]
        public void Overwritten_stream_is_valid_XML()
        {
            MemoryStream ms = new MemoryStream();
            ms.Write(UTF8Encoding.UTF8.GetBytes(xmlExample), 0, UTF8Encoding.UTF8.GetByteCount(xmlExample));
            StreamProfileStore sps = new StreamProfileStore(ms, new XMLProfileEncoder());
            var profiles = sps.Retrieve().ToList();
            profiles.Add(Profile.CreateDHCPProfile("additional test profile"));
            sps.Store(profiles);
            string retrievedXML = UTF8Encoding.UTF8.GetString(ms.GetBuffer()).Trim().Replace("\0", "");

            XDocument.Parse(retrievedXML);

        }

        [TestMethod]
        public void Overwritten_shorter_stream_is_valid_XML()
        {
            MemoryStream ms = new MemoryStream();
            ms.Write(UTF8Encoding.UTF8.GetBytes(xmlExample), 0, UTF8Encoding.UTF8.GetByteCount(xmlExample));
            StreamProfileStore sps = new StreamProfileStore(ms, new XMLProfileEncoder());
            var profiles = sps.Retrieve().ToList();
            var extraTestProfile = Profile.CreateDHCPProfile("additional test profile");
            profiles.Add(extraTestProfile);
            sps.Store(profiles);

            var test = sps.Retrieve();
            profiles.Remove(extraTestProfile);
            sps.Store(profiles);
            string retrievedXML = UTF8Encoding.UTF8.GetString(ms.GetBuffer().Take((int)ms.Length).ToArray()).Trim().Replace("\0", "");

            XDocument.Parse(retrievedXML);

        }

    }
}
