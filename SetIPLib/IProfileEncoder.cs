using System.Collections.Generic;

namespace SetIPLib {

    /// <summary>
    /// Used to encode a Profile to a byte array or decode a byte array to 
    /// a list of profiles.  This is provides translation between profiles
    /// and the persistence layer.
    /// </summary>
    public interface IProfileEncoder {
        byte[] Header { get; }
        byte[] Footer { get; }
        byte[] Encode(Profile p);
        IEnumerable<Profile> Decode(byte[] contents);
    }
}
