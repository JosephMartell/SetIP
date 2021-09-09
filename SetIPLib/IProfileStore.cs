using System.Collections.Generic;

namespace SetIPLib
{
    public interface IProfileStore
    {
        IProfileEncoder Encoder { get; }
        IEnumerable<Profile> Retrieve();
        void Store(IEnumerable<Profile> profiles);
    }
}
