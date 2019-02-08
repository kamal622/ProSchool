using System;
using System.Collections.Generic;
using ProSchool.Core;

namespace ProSchool.Data.Models
{
    public partial class Setting : BaseEntity
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
