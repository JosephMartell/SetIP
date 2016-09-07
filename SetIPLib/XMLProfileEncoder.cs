using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Linq;

namespace SetIPLib {
    class XMLProfileEncoder : IProfileEncoder {
        public byte[] Header {
            get {

                return Encoding.UTF8.GetBytes("<?xml version=\"1.0\" encoding=\"UTF - 8\" standalone=\"no\" ?><Profiles>");
            }
        }

        public byte[] Footer {
            get {
                return Encoding.UTF8.GetBytes("</Profiles>");
            }
        }

        public byte[] Encode(Profile p) {
            if (p.UseDHCP) {
                return Encoding.UTF8.GetBytes(EncodeDHCP(p));
            }
            else {
                return Encoding.UTF8.GetBytes(EncodeStatic(p));
            }
        }

        private string EncodeDHCP(Profile p) {
            return string.Format("<profile name=\"{0}\" useDHCP=\"true\" />", p.Name);
        }

        private string EncodeStatic(Profile p) {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<profile name=\"{0}\" useDHCP=\"false\">", p.Name);
            sb.AppendFormat("<ip>{0}</ip>", p.IP.ToString());
            sb.AppendFormat("<subnet>{0}</subnet>", p.Subnet.ToString());
            sb.Append("</profile>");

            return sb.ToString();
        }

        public IEnumerable<Profile> Decode(byte[] contents) {
            XDocument doc = XDocument.Parse(Encoding.UTF8.GetString(contents));
            var xmlProfiles = doc.Element("Profiles")
                                 .Elements("profile");

            string name = string.Empty;
            IPAddress ip = IPAddress.None;
            IPAddress subnet = IPAddress.None;
            List<Profile> profiles = new List<Profile>();
            foreach (var xmlProfile in xmlProfiles) {
                name = xmlProfile.Attribute("name").Value;
                if (xmlProfile.Attribute("useDHCP").Value == "true") {
                    profiles.Add(new Profile(name));
                }
                else {
                    ip = IPAddress.Parse(xmlProfile.Element("ip").Value);
                    subnet = IPAddress.Parse(xmlProfile.Element("subnet").Value);
                    profiles.Add(new Profile(name, ip, subnet));
                }
            }
            return profiles;
        }
    }
}
