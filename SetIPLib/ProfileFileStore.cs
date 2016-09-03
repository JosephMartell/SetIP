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

        public ProfileFileStore(string filePath) {
        }

        public ProfileFileStore(string filePath, IProfileEncoder encoder) {

        }

    }
}
