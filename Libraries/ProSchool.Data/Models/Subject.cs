using System;
using System.Collections.Generic;
using ProSchool.Core;

namespace ProSchool.Data.Models
{
    public partial class Subject : BaseEntity
    {
        public Subject()
        {
            this.Batches = new List<Batch>();
            this.Inquiries = new List<Inquiry>();
            this.StudentPrograms = new List<StudentProgram>();
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<Batch> Batches { get; set; }
        public virtual ICollection<Inquiry> Inquiries { get; set; }
        public virtual ICollection<StudentProgram> StudentPrograms { get; set; }
    }
}
