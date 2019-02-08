using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProSchool.Services.Models
{
    public class InvoiceDataset
    {
        public int Id { get; set; }
        public string InvoiceNo { get; set; }
        public int StudentId { get; set; }
        public int StudentProgramId { get; set; }
        public int SubjectId { get; set; }
        public int DivisionId { get; set; }
        public int BatchId { get; set; }
        public int Frequency { get; set; }
        public string FrequencyName { get; set; }
        public string StudentName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Mobile { get; set; }
        public int PaymentStatus { get; set; }
        public bool IsPaid { get; set; }
        public string SubjectName { get; set; }
        public string DivisionName { get; set; }
        public string BatchName { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string InstitutionName { get; set; }
        public string InstitutionAddress { get; set; }
        public string InstitutionCity { get; set; }
        public string InstitutionState { get; set; }
        public string InstitutionPincode { get; set; }
        public string InstitutionPhone { get; set; }
        public bool ISActive { get; set; }
        public decimal InvoiceAmount { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime? PaymentDate { get; set; }
        public int Tax { get; set; }
        public DateTime PeriodStartDate { get; set; }
        public DateTime PeriodEndDate { get; set; }
    }
}
