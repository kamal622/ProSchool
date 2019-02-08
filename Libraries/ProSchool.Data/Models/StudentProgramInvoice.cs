using System;
using System.Collections.Generic;
using ProSchool.Core;

namespace ProSchool.Data.Models
{
    public partial class StudentProgramInvoice : BaseEntity
    {
        public int StudentProgramId { get; set; }
        public string InvoiceNo { get; set; }
        public int PaymentStatus { get; set; }
        public Nullable<int> PaymentType { get; set; }
        public string chequeNo { get; set; }
        public Nullable<System.DateTime> PaymentDate { get; set; }
        public string BankName { get; set; }
        public string IFSCCode { get; set; }
        public bool IsChequeClear { get; set; }
        public System.DateTime InvoiceDate { get; set; }
        public decimal InvoiceAmount { get; set; }
        public System.DateTime PeriodStartDate { get; set; }
        public System.DateTime PeriodEndDate { get; set; }
        public virtual StudentProgram StudentProgram { get; set; }
    }
}
