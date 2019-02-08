using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProSchool.Services.Models
{
  public  class StudentProgramInvoiceViewModel
    {
        public int Id { get; set; }
        public int FeeId { get; set; }
        public int StudentProgramId { get; set; }
        public int PaymentStatus { get; set; }
        public int PaymentType { get; set; }
        public string chequeNo { get; set; }
        public string BankName { get; set; }
        public string IFSCCode { get; set; }
        public bool IsChequeClear { get; set; }
        public DateTime InvoiceDate { get; set; }
        public int InvoiceAmount { get; set; }
    }
}
