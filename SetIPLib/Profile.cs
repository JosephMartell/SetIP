using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace SetIPLib {

    public class Profile {
        private string _name;

        public string Name {
            get { return _name; }
        }

        private IPAddress _ip;

        public IPAddress IP {
            get { return _ip; }
        }

        private IPAddress _subnet;

        public IPAddress Subnet {
            get { return _subnet; }
        }

        public Profile(string name, IPAddress ip, IPAddress subnet) {
            _name = name;
            _ip = ip;
            _subnet = subnet;
        }
    }
}
