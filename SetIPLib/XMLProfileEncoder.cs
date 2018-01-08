using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Linq;

namespace SetIPLib
{
    public class XMLProfileEncoder : IProfileEncoder
    {
        public byte[] Header
        {
            get
            {
                return Encoding.UTF8.GetBytes("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\" ?><Profiles>");
            }
        }

        public byte[] Footer
        {
            get
            {
                return Encoding.UTF8.GetBytes("</Profiles>");
            }
        }

        public IEnumerable<Profile> Decode(byte[] contents)
        {
            var xmlProfiles = GetProfileXMLElements(contents);
            var profiles = xmlProfiles.Select(p => ParseProfileXML(p));
            return profiles;
        }

        private IEnumerable<XElement> GetProfileXMLElements(byte[] xmlStringAsBytes)
        {
            string xmlString = Encoding.UTF8.GetString(xmlStringAsBytes);
            try
            {
                XDocument document = XDocument.Parse(xmlString);
                return document.Element("Profiles").Elements("profile");
            }
            catch (Exception)
            {
                throw new System.Xml.XmlException("The profile storage file was not properly formatted.");
            }
        }

        private bool XmlProfileIsDHCP(XElement xmlProfile)
        {
            return xmlProfile.Attribute("useDHCP").Value == "true";
        }

        private Profile ParseProfileXML(XElement xmlProfile)
        {
            if (XmlProfileIsDHCP(xmlProfile))
            {
                return ParseDHCPProfileXML(xmlProfile);
            }
            else
            {
                return ParseStaticAddressProfileXML(xmlProfile);
            }
        }

        private Profile ParseDHCPProfileXML(XElement xmlProfile)
        {
            string name = xmlProfile.Attribute("name").Value;
            return Profile.CreateDHCPProfile(name);
        }

        private Profile ParseStaticAddressProfileXML(XElement xmlProfile)
        {
            string name = xmlProfile.Attribute("name").Value;
            IPAddress ip = IPAddress.Parse(xmlProfile.Element("ip").Value);
            IPAddress subnet = IPAddress.Parse(xmlProfile.Element("subnet").Value);
            IPAddress gw = ParseStaticGWFromXML(xmlProfile.Element("gateway"));
            List<IPAddress> DNSServers = ParseStaticDNSServersFromXML(xmlProfile.Element("DNSServers"));
            return ConstructStaticProfile(name, ip, subnet, gw, DNSServers);
        }

        private IPAddress ParseStaticGWFromXML(XElement gwElement)
        {
            if (gwElement == null)
            {
                return IPAddress.None;
            }
            else
            {
                return IPAddress.Parse(gwElement.Value);
            }
        }

        private List<IPAddress> ParseStaticDNSServersFromXML(XElement dnsServersElement)
        {
            List<IPAddress> DNSServers = new List<IPAddress>();
            if (dnsServersElement != null)
            {
                foreach (var server in dnsServersElement.Elements("server"))
                {
                    DNSServers.Add(IPAddress.Parse(server.Value));
                }
            }
            return DNSServers;
        }

        private Profile ConstructStaticProfile(string name, IPAddress ip, IPAddress subnet, IPAddress gw, List<IPAddress> dnsServers)
        {
            if (gw == IPAddress.None)
            {
                return Profile.CreateStaticProfile(name, ip, subnet);
            }
            else if (dnsServers.Count == 0)
            {
                return Profile.CreateStaticProfile(name, ip, subnet, gw);
            }
            else
            {
                return Profile.CreateStaticProfile(name, ip, subnet, gw, dnsServers);
            }
        }

        public byte[] Encode(Profile p)
        {
            if (p.UseDHCP)
            {
                return Encoding.UTF8.GetBytes(EncodeDHCP(p));
            }
            else
            {
                return Encoding.UTF8.GetBytes(EncodeStatic(p));
            }
        }

        private string EncodeDHCP(Profile p)
        {
            return string.Format($"<profile name=\"{p.Name}\" useDHCP=\"true\" />");
        }

        private string EncodeStatic(Profile p)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"<profile name=\"{p.Name}\" useDHCP=\"false\">");
            sb.Append(EncodeIP(p));
            sb.Append(EncodeSubet(p));
            sb.Append(EncodeGateway(p));
            sb.Append(EncodeDNS(p));
            sb.Append("</profile>");

            return sb.ToString();
        }

        private string EncodeIP(Profile p)
        {
            return $"<ip>{p.IP.ToString()}</ip>";
        }

        private string EncodeSubet(Profile p)
        {
            return $"<subnet>{p.Subnet.ToString()}</subnet>";
        }

        private string EncodeGateway(Profile p)
        {
            if ((p.UseDHCP) || (p.Gateway == IPAddress.None))
            {
                return string.Empty;
            }
            else
            {
                return $"<gateway>{p.Gateway.ToString()}</gateway>";
            }
        }

        private string EncodeDNS(Profile p)
        {
            if (p.DNSServers.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<DNSServers>");
                foreach (IPAddress dns in p.DNSServers)
                {
                    sb.Append($"<server>{dns.ToString()}</server>");
                }
                sb.Append("</DNSServers>");
                return sb.ToString();
            }
            else
            {
                return string.Empty;
            }
        }

    }
}
