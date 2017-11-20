using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SetIPLib
{
    public class StreamProfileStore :
        IProfileStore,
        IDisposable
    {
        public IProfileEncoder Encoder { get; }
        public Stream FileStream { get; }

        public IEnumerable<Profile> Retrieve()
        {
            byte[] buffer = new byte[FileStream.Length];
            FileStream.Position = 0;
            FileStream.Read(buffer, 0, (int)FileStream.Length);
            return Encoder.Decode(buffer);
        }

        public void Store(IEnumerable<Profile> profiles)
        {
            FileStream.Write(Encoder.Header, 0, Encoder.Header.Length);
            foreach (var p in profiles)
            {
                var pBuffer = Encoder.Encode(p);
                FileStream.Write(pBuffer, 0, pBuffer.Length);
            }
            FileStream.Write(Encoder.Footer, 0, Encoder.Footer.Length);
            throw new NotImplementedException();
        }

        public StreamProfileStore(Stream fileStream, IProfileEncoder encoder)
        {
            Encoder = encoder;
            FileStream = fileStream;
        }

        public void Dispose()
        {
            FileStream.Dispose();
        }
    }
}
