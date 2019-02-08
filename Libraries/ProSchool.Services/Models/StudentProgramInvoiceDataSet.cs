using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProSchool.Services.Models
{
    public class StudentProgramInvoiceDataSet
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int StudentProgramId { get; set; }
        public string StudentName { get; set; }
        public string PaymentStatus { get; set; }
        public bool IsPaid { get; set; }
        public string Mobile { get; set; }
        public string Department { get; set; }
        public string Class { get; set; }
        public bool ISActive { get; set; }
        public decimal InvoiceAmount { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime? PaymentDate { get; set; }
    }

    public class StudentFeeDataSet
    {
        public int StudentProgramId { get; set; }
        public int Id { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal InvoiceAmount { get; set; }
        public string SubjectName { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string PaymentStatus { get; set; }
        public string PaymentType { get; set; }
        public bool IsPaid { get; set; }
    }
}
