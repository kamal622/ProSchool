using System;
using System.Collections.Generic;
using ProSchool.Core;

namespace ProSchool.Data.Models
{
    public partial class Fee : BaseEntity
    {
        public Fee()
        {
            this.StudentPrograms = new List<StudentProgram>();
        }

        public string Name { get; set; }
        public int Frequency { get; set; }
        public int Amount { get; set; }
        public virtual FeeFrequency FeeFrequency { get; set; }
        public virtual ICollection<StudentProgram> StudentPrograms { get; set; }
    }
}
