using System;
using System.Collections.Generic;
using ProSchool.Core;

namespace ProSchool.Data.Models
{
    public partial class Section : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
