using CLImber;
using SetIPLib;
using System;
using System.Linq;
using System.Net;

namespace SetIPCLI
{
    [CommandClass("edit", ShortDescription = "Updates a profile with the specified information. Updated info is specified using options: --ip=xxx.xxx.xxx.xxx")]
    class EditProfile
    {
        private Profile _editingProfile;

        private IPAddress _ip = null;
        [CommandOption("ip")]
        public IPAddress IP
        {
            get
            {
                return _ip ?? _editingProfile?.IP;
            }
            set
            {
                _ip = value;
            }
        }

        private IPAddress _subnet = null;
        [CommandOption("subnet")]
        public IPAddress Subnet
        {
            get
            {
                return _subnet ?? _editingProfile?.Subnet;
            }
            set
            {
                _subnet = value;
            }
        }

        private IPAddress _gateway = null;
        [CommandOption("gw")]
        [CommandOption("gateway")]
        public IPAddress Gateway
        {
            get
            {
                return _gateway ?? _editingProfile?.Gateway;
            }
            set
            {
                _gateway = value;
            }
        }

        private IPAddress _dns = null;
        [CommandOption("dns")]
        public IPAddress DNS
        {
            get
            {
                return _dns ?? _editingProfile?.DNSServers.DefaultIfEmpty(IPAddress.Any).FirstOrDefault();
            }
            set
            {
                _dns = value;
            }
        }

        private string _name = null;
        [CommandOption("name")]
        public string Name
        {
            get
            {
                return _name ?? _editingProfile?.Name;
            }
            set
            {
                _name = value;
            }
        }

        [CommandOption("dhcp")]
        public bool DHCP { get; set; } = false;

        private readonly IProfileStore _store;
        public EditProfile(IProfileStore profileStore)
        {
            _store = profileStore;
        }

        [CommandHandler(ShortDescription = "Update a profile with new values.")]
        public void Edit(string profileName)
        {
            var profiles = _store.Retrieve().ToList();
            var possibleProfiles = profiles.Where(p => p.Name.Equals(profileName, StringComparison.OrdinalIgnoreCase));

            if (possibleProfiles.Count() != 1)
            {
                return;
            }

            _editingProfile = possibleProfiles.First();
            Profile newProfile;
            if (DHCP)
            {
                newProfile = Profile.CreateDHCPProfile(Name);
            }
            if (DNS != IPAddress.Any)
            {
                newProfile = Profile.CreateStaticProfile(Name, IP, Subnet, Gateway, new IPAddress[] { DNS });
            }
            if (Gateway != IPAddress.Any)
            {
                newProfile = Profile.CreateStaticProfile(Name, IP, Subnet, Gateway);
            }
            else
            {
                newProfile = Profile.CreateStaticProfile(Name, IP, Subnet);
            }

            profiles.Remove(_editingProfile);
            profiles.Add(newProfile);
            _store.Store(profiles);
        }


    }
}
