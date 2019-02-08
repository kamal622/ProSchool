using System;
using System.Collections.Generic;
using ProSchool.Core;

namespace ProSchool.Data.Models
{
    public partial class UserLogin : BaseEntity
    {
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}
