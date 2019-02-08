using System;
using System.Collections.Generic;
using ProSchool.Core;

namespace ProSchool.Data.Models
{
    public partial class Department : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
