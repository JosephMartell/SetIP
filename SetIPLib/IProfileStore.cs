using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SetIPLib {
    interface IProfileStore {
        IProfileEncoder Encoder { get; }
        IEnumerable<Profile> Retrieve();
        void Store(IEnumerable<Profile> profiles);
    }
}
