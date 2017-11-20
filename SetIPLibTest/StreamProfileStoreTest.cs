using System.IO;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SetIPLib;
using System.Collections.Generic;
using System.Linq;

namespace SetIPLibTest
{
    [TestClass]
    public class StreamProfileStoreTest
    {
        [TestMethod]
        public void Decodes_example_profile_correctly()
        {
            string xmlExample = "<?xml version=\"1.0\" encoding=\"UTF - 8\" standalone=\"no\" ?><Profiles><profile name=\"test profile\" useDHCP=\"true\" /> </Profiles>";
        }
    }
}
