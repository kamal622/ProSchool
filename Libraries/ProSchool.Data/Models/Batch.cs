using System;
using System.Collections.Generic;
using ProSchool.Core;

namespace ProSchool.Data.Models
{
    public partial class Batch : BaseEntity
    {
        public Batch()
        {
            this.StudentPrograms = new List<StudentProgram>();
        }

        public int SubjectId { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public string Description { get; set; }
        public virtual Subject Subject { get; set; }
        public virtual ICollection<StudentProgram> StudentPrograms { get; set; }
    }
}
