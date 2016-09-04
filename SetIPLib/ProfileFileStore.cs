using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SetIPLib {


    /// <summary>
    /// Allows the storage and retrieval of Profiles to a file as XML.
    /// </summary>
    /// 
    //TODO: extract an interface so other storage types could be created, if desired.
    public class ProfileFileStore {

        private System.IO.TextWriter tw;

        private string _filePath;

        public string FilePath {
            get { return _filePath; }
        }

        public void Store(Profile profile) {

        }

        /// <summary>
        /// Uses the default appdata path to store profiles with 
        /// the default XML Profile encoder.
        /// </summary>
        public ProfileFileStore() {

        }

        /// <summary>
        /// Uses the default XML profile encoder to store profiles
        /// at the specified path
        /// </summary>
        /// <param name="filePath">Full path to file, including file name.</param>
        public ProfileFileStore(string filePath) {
        }

        /// <summary>
        /// Stores profiles at the specified file path using the
        /// provided encoder.
        /// </summary>
        /// <param name="filePath">Full path to file, including file name.</param>
        /// <param name="encoder">Encoder used to write profile information to the specified file.</param>
        public ProfileFileStore(string filePath, IProfileEncoder encoder) {

        }

    }
}
