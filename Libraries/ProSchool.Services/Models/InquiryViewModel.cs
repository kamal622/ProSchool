using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProSchool.Services.Models
{
  public  class InquiryViewModel
    {
        public int Id { get; set; }
        public string Remark { get; set; }
        public int DivisionId { get; set; }
        public int SubjectId { get; set; }
    }
}
