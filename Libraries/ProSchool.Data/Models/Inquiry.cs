using System;
using System.Collections.Generic;
using ProSchool.Core;

namespace ProSchool.Data.Models
{
    public partial class Inquiry : BaseEntity
    {
        public Inquiry()
        {
            this.Students = new List<Student>();
        }

        public System.DateTime InquiryDate { get; set; }
        public string Remark { get; set; }
        public int DivisionId { get; set; }
        public int SubjectId { get; set; }
        public virtual Division Division { get; set; }
        public virtual Subject Subject { get; set; }
        public virtual ICollection<Student> Students { get; set; }
    }
}
