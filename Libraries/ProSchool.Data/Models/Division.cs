using System;
using System.Collections.Generic;
using ProSchool.Core;

namespace ProSchool.Data.Models
{
    public partial class Division : BaseEntity
    {
        public Division()
        {
            this.Inquiries = new List<Inquiry>();
            this.StudentPrograms = new List<StudentProgram>();
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Inquiry> Inquiries { get; set; }
        public virtual ICollection<StudentProgram> StudentPrograms { get; set; }
    }
}
