using System.Net;

namespace SetIPLib {

    public class Profile {
        private string _name;

        public string Name {
            get { return _name; }
        }

        protected readonly bool _useDHCP;

        public bool UseDHCP {
            get { return _useDHCP; }
        }

        private IPAddress _ip;

        public IPAddress IP {
            get { return _ip; }
        }

        private IPAddress _subnet;

        public IPAddress Subnet {
            get { return _subnet; }
        }

        public Profile(string name) {
            _name = name;
            _useDHCP = true;
            _ip = IPAddress.None;
            _subnet = IPAddress.None;
        }

        public Profile(string name, IPAddress ip, IPAddress subnet) {
            _name = name;
            _ip = ip;
            _subnet = subnet;
        }
    }
}
