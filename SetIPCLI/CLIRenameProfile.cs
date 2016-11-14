using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SetIPLib;

namespace SetIPCLI {
    class CLIRenameProfile : ICLICommand {
        public ArgumentGroup Arguments { get; }

        public string OldName { get; }
        public string NewName { get; }

        public void Execute(ref IProfileStore store) {
            var target = (from p in store.Retrieve()
                          where p.Name.ToUpper() == OldName.ToUpper()
                          select p).First();

            if (target != null) {
                Profile renamed;
                if (target.UseDHCP) {
                    renamed = new Profile(NewName);
                }
                else {
                    renamed = new Profile(NewName, target.IP, target.Subnet);
                }
                var newSet = (from p in store.Retrieve()
                              where p.Name.ToUpper() != OldName.ToUpper()
                              select p).ToList();
                newSet.Add(renamed);
                store.Store(newSet);
            }
        }

        public CLIRenameProfile(ArgumentGroup args) {
            Arguments = args;
            var argEnum = args.Arguments.GetEnumerator();
            if (argEnum.MoveNext()) {
                OldName = argEnum.Current;
            }
            else {
                OldName = string.Empty;
            }

            if (argEnum.MoveNext()) {
                NewName = argEnum.Current;
            }
            else {
                NewName = string.Empty;
            }

        }
    }
}
