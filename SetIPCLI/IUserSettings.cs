namespace SetIPCLI
{
    public interface IUserSettings
    {
        string DefaultNIC { get; set; }
        string ProfileFileLocation { get; set; }
    }
}