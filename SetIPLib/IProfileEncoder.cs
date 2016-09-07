using System.Collections.Generic;

namespace SetIPLib {

    /// <summary>
    /// Used to encode a Profile to a byte array or decode a byte array to 
    /// a list of profiles.  This is provided so that the profiles can be stored and retrieved from disk.
    /// </summary>
    public interface IProfileEncoder {
        byte[] Header { get; }
        byte[] Footer { get; }
        byte[] Encode(Profile p);
        IEnumerable<Profile> Decode(byte[] contents);
    }
}
