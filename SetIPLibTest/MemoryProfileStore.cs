using SetIPLib;
using System.Collections.Generic;
using System.Linq;

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
