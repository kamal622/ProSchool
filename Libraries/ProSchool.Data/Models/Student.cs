using System;
using System.Collections.Generic;
using ProSchool.Core;

namespace ProSchool.Data.Models
{
    public partial class Student : BaseEntity
    {
        public Student()
        {
            this.StudentPrograms = new List<StudentProgram>();
        }

        public int InquiryId { get; set; }
        public string RegistrationNo { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string Mobile { get; set; }
        public System.DateTime DOB { get; set; }
        public string Email { get; set; }
        public string Remarks { get; set; }
        public string School { get; set; }
        public string College { get; set; }
        public string SCAddress { get; set; }
        public string ContactName { get; set; }
        public string ContactNumber { get; set; }
        public string ContactRelation { get; set; }
        public string RelativePath { get; set; }
        public string OriginalFileName { get; set; }
        public string SystemFileName { get; set; }
        public bool IsActive { get; set; }
        public bool IsRegistered { get; set; }
        public virtual Inquiry Inquiry { get; set; }
        public virtual ICollection<StudentProgram> StudentPrograms { get; set; }
    }
}
