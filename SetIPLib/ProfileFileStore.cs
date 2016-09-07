using System;
using System.Collections.Generic;

namespace SetIPLib {


    /// <summary>
    /// Allows the storage and retrieval of Profiles to a file as XML.
    /// </summary>
    /// 
    public class ProfileFileStore : IProfileStore {

        private string _filePath;

        public string FilePath {
            get { return _filePath; }
        }

        protected readonly IProfileEncoder _encoder;

        public IProfileEncoder Encoder {
            get { return _encoder; }
        }

        public IEnumerable<Profile> Retrieve() {
            using (System.IO.FileStream fs = new System.IO.FileStream(FilePath, System.IO.FileMode.Open)) {
                byte[] contents = new byte[fs.Length];
                try {
                    if (fs.Length < int.MaxValue) {
                        fs.Read(contents, 0, (int)fs.Length);
                    }

                }
                finally {
                    if (fs != null) {
                        fs.Close();
                    }
                }
                return Encoder.Decode(contents);
            }
        }

        public void Store(IEnumerable<Profile> profiles) {
            using (System.IO.FileStream fs = new System.IO.FileStream(FilePath, System.IO.FileMode.Create)) {
                try {
                    fs.Write(Encoder.Header, 0, Encoder.Header.Length);

                    foreach (var p in profiles) {
                        byte[] pBytes = Encoder.Encode(p);
                        fs.Write(pBytes, 0, pBytes.Length);
                    }
                    fs.Write(Encoder.Footer, 0, Encoder.Footer.Length);
                }
                finally {
                    if (fs != null) {
                        fs.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Uses the default appdata path to store profiles with 
        /// the default XML Profile encoder.
        /// </summary>
        public ProfileFileStore() {
            var directory = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SetIP");
            System.IO.Directory.CreateDirectory(directory);

            _filePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SetIP\\profiles.xml");
            _encoder = new XMLProfileEncoder();
        }

        /// <summary>
        /// Uses the default XML profile encoder to store profiles
        /// at the specified path
        /// </summary>
        /// <param name="filePath">Full path to file, including file name.</param>
        public ProfileFileStore(string filePath) {
            _filePath = filePath;
            _encoder = new XMLProfileEncoder();
        }

        /// <summary>
        /// Stores profiles at the specified file path using the
        /// provided encoder.
        /// </summary>
        /// <param name="filePath">Full path to file, including file name.</param>
        /// <param name="encoder">Encoder used to write profile information to the specified file.</param>
        public ProfileFileStore(string filePath, IProfileEncoder encoder) {
            _filePath = filePath;
            _encoder = encoder;
        }

    }
}
