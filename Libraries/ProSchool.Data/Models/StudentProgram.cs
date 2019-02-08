using System;
using System.Collections.Generic;
using ProSchool.Core;

namespace ProSchool.Data.Models
{
    public partial class StudentProgram : BaseEntity
    {
        public StudentProgram()
        {
            this.StudentProgramInvoices = new List<StudentProgramInvoice>();
        }

        public int StudentId { get; set; }
        public int DivisionId { get; set; }
        public int SubjectId { get; set; }
        public int BatchId { get; set; }
        public int FeeId { get; set; }
        public System.DateTime RegistrationDate { get; set; }
        public System.DateTime NextDueDate { get; set; }
        public bool IsActive { get; set; }
        public virtual Batch Batch { get; set; }
        public virtual Division Division { get; set; }
        public virtual Fee Fee { get; set; }
        public virtual ICollection<StudentProgramInvoice> StudentProgramInvoices { get; set; }
        public virtual Student Student { get; set; }
        public virtual Subject Subject { get; set; }
    }
}
