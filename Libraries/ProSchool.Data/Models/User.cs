using System;
using System.Collections.Generic;
using ProSchool.Core;

namespace ProSchool.Data.Models
{
    public partial class User : BaseEntity
    {
        public User()
        {
            this.UserClaims = new List<UserClaim>();
            this.UserLogins = new List<UserLogin>();
            this.UserRoles = new List<UserRole>();
        }

        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public Nullable<System.DateTime> LockoutEndDateUtc { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public string UserName { get; set; }
        public virtual ICollection<UserClaim> UserClaims { get; set; }
        public virtual ICollection<UserLogin> UserLogins { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
