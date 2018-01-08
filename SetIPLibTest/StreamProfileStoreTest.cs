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
        MemoryStream storageStream;
        StreamProfileStore profileStore;


        [TestInitialize]
        public void Create_stream_and_stream_profile_store()
        {
            storageStream = new MemoryStream();
            profileStore = new StreamProfileStore(storageStream, new XMLProfileEncoder());
        }


        [TestMethod]
        public void Decodes_example_profile_correctly()
        {
            PopulateMemoryStream(storageStream);
            var profiles = profileStore.Retrieve();
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
            List<Profile> profiles = new List<Profile>() { examplelProfile };

            profileStore.Store(profiles);

            Assert.AreEqual(xmlExample, 
                ExtractXMLFromMemoryStream(storageStream));
        }

        [TestMethod]
        public void Overwritten_stream_is_valid_XML()
        {
            PopulateMemoryStream(storageStream);

            var profiles = profileStore.Retrieve().ToList();
            profiles.Add(Profile.CreateDHCPProfile("additional test profile"));
            profileStore.Store(profiles);

            //not sure how to better document this.  The "Assert" for this test would be that
            //Parse does not throw an exception (well formatted xml)
            XDocument.Parse(
                ExtractXMLFromMemoryStream(storageStream));
        }

        [TestMethod]
        public void Overwritten_shorter_stream_is_valid_XML()
        {
            PopulateMemoryStream(storageStream);
            var profiles = profileStore.Retrieve().ToList();
            var extraTestProfile = Profile.CreateDHCPProfile("additional test profile");
            profiles.Add(extraTestProfile);
            profileStore.Store(profiles);

            var test = profileStore.Retrieve();
            profiles.Remove(extraTestProfile);
            profileStore.Store(profiles);

            //not sure how to better document this.  The "Assert" for this test would be that
            //Parse does not throw an exception (well formatted xml)
            XDocument.Parse(
                ExtractXMLFromMemoryStream(storageStream));
        }

        public void PopulateMemoryStream(MemoryStream ms)
        {
            ms.Write(
                UTF8Encoding.UTF8.GetBytes(xmlExample), 
                0, 
                UTF8Encoding.UTF8.GetBytes(xmlExample).Count());
        }

        public string ExtractXMLFromMemoryStream(MemoryStream ms)
        {
            int msLength = (int)ms.Length;
            var bytes = ms.GetBuffer().Take(msLength).ToArray();

            return UTF8Encoding.UTF8.GetString(bytes).Trim();
        }
    }
}
