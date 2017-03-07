using SetIPLib;

namespace SetIPCLI {
    interface ICLICommand {
        ArgumentGroup Arguments{ get; }
        void Execute(ref IProfileStore store);
        string Help();
    }
}
