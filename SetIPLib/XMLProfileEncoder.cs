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
            } else {
                return Encoding.UTF8.GetBytes(EncodeStatic(p));
            }
        }

        private string EncodeDHCP(Profile p) {
            return string.Format($"<profile name=\"{p.Name}\" useDHCP=\"true\" />");
        }

        private string EncodeStatic(Profile p) {
            StringBuilder sb = new StringBuilder();
            sb.Append($"<profile name=\"{p.Name}\" useDHCP=\"false\">");
            sb.Append(EncodeIP(p));
            sb.Append(EncodeSubet(p));
            sb.Append(EncodeGateway(p));
            sb.Append(EncodeDNS(p));
            sb.Append("</profile>");

            return sb.ToString();
        }

        private string EncodeIP(Profile p) {
            return $"<ip>{p.IP.ToString()}</ip>";
        }

        private string EncodeSubet(Profile p) {
            return $"<subnet>{p.Subnet.ToString()}</subnet>";
        }

        private string EncodeGateway(Profile p) {
            if ((p.UseDHCP) || (p.Gateway == IPAddress.None)) {
                return string.Empty;
            } else {
                return $"<gateway>{p.Gateway.ToString()}</gateway>";
            }
        }

        private string EncodeDNS(Profile p) {
            if (p.DNSServers.Count > 0) {
                StringBuilder sb = new StringBuilder();
                sb.Append("<DNSServers>");
                foreach (IPAddress dns in p.DNSServers) {
                    sb.Append($"<server>{dns.ToString()}</server>");
                }
                sb.Append("</DNSServers>");
                return sb.ToString();
            } else {
                return string.Empty;
            }
        }

        public IEnumerable<Profile> Decode(byte[] contents) {
            string xmlString = Encoding.UTF8.GetString(contents);
            if ((xmlString == null) || (xmlString == string.Empty)) {
                return new List<Profile>();
            }
            XDocument doc = XDocument.Parse(xmlString);
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
                    XElement el = xmlProfile.Element("gateway");
                    IPAddress gw = IPAddress.None;
                    if (el != null) {
                        gw = IPAddress.Parse(el.Value);
                    }
                    List<IPAddress> DNSServers = new List<IPAddress>();
                    el = xmlProfile.Element("DNSServers");
                    if (el != null) {
                        foreach (var server in xmlProfile.Element("DNSServers").Elements("servers")) {
                            DNSServers.Add(IPAddress.Parse(server.Value));
                        }
                    }
                    if (gw == IPAddress.None) {
                        profiles.Add(new Profile(name, ip, subnet));
                    } else if (DNSServers.Count == 0) {
                        profiles.Add(new Profile(name, ip, subnet, gw));
                    } else {
                        profiles.Add(new Profile(name, ip, subnet, gw, DNSServers));
                    }
                }
            }
            return profiles;
        }
    }
}
