using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SetIPCLI {
    class UnknownCommandException : Exception {
        public UnknownCommandException(string argument) 
            : base(string.Format("An unknown command line argument was supplied: {0}", argument)) {
        }            
    }
}
