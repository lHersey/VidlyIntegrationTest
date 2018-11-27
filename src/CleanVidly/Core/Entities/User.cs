using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CleanVidly.Core.Entities
{
    public class User
    {
        public User()
        {
            UserRoles = new Collection<UserRole>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public byte[] Salt { get; set; }
        public byte[] Password { get; set; }

        public DateTime JoinDate { get; set; }

        public ICollection<UserRole> UserRoles { get; private set; }
    }
}