using System;
using System.Collections.Generic;
using ProSchool.Core;

namespace ProSchool.Data.Models
{
    public partial class FeeFrequency : BaseEntity
    {
        public FeeFrequency()
        {
            this.Fees = new List<Fee>();
        }

        public string Description { get; set; }
        public int Frequency { get; set; }
        public virtual ICollection<Fee> Fees { get; set; }
    }
}
