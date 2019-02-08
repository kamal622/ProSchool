using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProSchool.Services.Models
{
   public class RegistrationDataSet
    {
        public int Id { get; set;}
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Department { get; set; }
        public string Class { get; set; }
        public string Section { get; set; }
        public DateTime NextDueDate { get; set; }
        public string Status { get; set; }
        public int DivisionId { get; set; }
        public int SubjectId { get; set; }
        public int BatchId { get; set; }
        public bool Active { get; set; }
       
    }
}
