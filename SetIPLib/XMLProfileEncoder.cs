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
                return Encoding.UTF8.GetBytes("<?xml version=\"1.0\" encoding=\"UTF - 8\" standalone=\"no\" ?><Profiles>");
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
            var profiles = from p in xmlProfiles
                     select ParseProfileXML(p);
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
            return new Profile(name);
        }

        private Profile ParseStaticAddressProfileXML(XElement xmlProfile)
        {
            string name = xmlProfile.Attribute("name").Value;
            IPAddress ip = IPAddress.Parse(xmlProfile.Element("ip").Value);
            IPAddress subnet = IPAddress.Parse(xmlProfile.Element("subnet").Value);
            XElement el = xmlProfile.Element("gateway");
            IPAddress gw = IPAddress.None;
            if (el != null)
            {
                gw = IPAddress.Parse(el.Value);
            }
            List<IPAddress> DNSServers = new List<IPAddress>();
            el = xmlProfile.Element("DNSServers");
            if (el != null)
            {
                foreach (var server in xmlProfile.Element("DNSServers").Elements("server"))
                {
                    DNSServers.Add(IPAddress.Parse(server.Value));
                }
            }
            if (gw == IPAddress.None)
            {
                return new Profile(name, ip, subnet);
            }
            else if (DNSServers.Count == 0)
            {
                return new Profile(name, ip, subnet, gw);
            }
            else
            {
                return new Profile(name, ip, subnet, gw, DNSServers);
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
