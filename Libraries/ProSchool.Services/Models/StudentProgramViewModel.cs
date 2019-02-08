using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProSchool.Services.Models
{
    public class StudentProgramViewModel
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int DivisionId { get; set; }
        public int SubjectId { get; set; }
        public int BatchId { get; set; }
        public int FeeId { get; set; }
    }
}
