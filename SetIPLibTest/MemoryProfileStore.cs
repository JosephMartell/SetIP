using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SetIPLib;

namespace SetIPLibTest
{
    class MemoryProfileStore :
        IProfileStore
    {
        public IProfileEncoder Encoder { get; }

        private List<Profile> _profiles = new List<Profile>();

        public IEnumerable<Profile> Retrieve()
        {
            return _profiles;
        }

        public void Store(IEnumerable<Profile> profiles)
        {
            _profiles = profiles.ToList();
        }
    }
}
