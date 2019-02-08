using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProSchool.Services.Models
{
   public class StudentDataSet
    {
        public int Id { get; set; }
        public string StudentName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Mobile { get; set; }
        public bool IsActive { get; set; }
        public string Programs { get; set; }
        public int DivisionId { get; set; }
        public int SubjectId { get; set; }
        public int BatchId { get; set; }
    }
}
