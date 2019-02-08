using System;
using System.Collections.Generic;
using ProSchool.Core;

namespace ProSchool.Data.Models
{
    public partial class Role : BaseEntity
    {
        public Role()
        {
            this.UserRoles = new List<UserRole>();
        }

        public string Name { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
