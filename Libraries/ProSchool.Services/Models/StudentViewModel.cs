using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProSchool.Services.Models
{
  public  class StudentViewModel
    {
        public int Id { get; set; }
        public int InquiryId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public char Gender { get; set; }
        public string Address { get; set; }
        public string Mobile { get; set; }
        public DateTime DOB { get; set; }
        public string Email { get; set; }
        public string Remarks { get; set; }
        public string School { get; set; }
        public string College { get; set; }
        public string SCAddress { get; set; }
        public string ContactName { get; set; }
        public string ContactNumber { get; set; }
        public string ContactRelation { get; set; }
        public string RegistrationNo { get; set; }
    }
}
