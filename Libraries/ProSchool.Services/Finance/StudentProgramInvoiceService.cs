using ProSchool.Core.Data;
using ProSchool.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProSchool.Services.Finance
{
    public class StudentProgramInvoiceService
    {
        private readonly IRepository<Data.Models.StudentProgramInvoice> _studentprograminvoiceRepository;
        private readonly IRepository<Data.Models.Fee> _feeRepository;
        private readonly IRepository<Data.Models.StudentProgram> _studentProgramRepository;
        private readonly IRepository<Data.Models.FeeFrequency> _feeFrequencyRepository;
        private readonly IRepository<Data.Models.Student> _studentRepository;
        private readonly IRepository<Data.Models.Setting> _settingRepository;

        public StudentProgramInvoiceService(IRepository<Data.Models.StudentProgramInvoice> studentprograminvoiceRepository,
            IRepository<Data.Models.Fee> feeRepository, IRepository<Data.Models.StudentProgram> studentProgramRepository,
            IRepository<Data.Models.FeeFrequency> feeFrequencyRepository, IRepository<Data.Models.Student> studentRepository,
            IRepository<Data.Models.Setting> settingRepository)
        {
            this._studentprograminvoiceRepository = studentprograminvoiceRepository;
            this._feeRepository = feeRepository;
            this._studentProgramRepository = studentProgramRepository;
            this._feeFrequencyRepository = feeFrequencyRepository;
            this._studentRepository = studentRepository;
            this._settingRepository = settingRepository;
        }
        public int Insert(Data.Models.StudentProgramInvoice studentprograminvoice)
        {
            //if (this._studentprograminvoiceRepository.Table.Any(a => a.StudentProgramId == studentprograminvoice.StudentProgramId))
            // return 101;

            this._studentprograminvoiceRepository.Insert(studentprograminvoice);

            var existing = this._studentprograminvoiceRepository.GetById(studentprograminvoice.Id);
            existing.InvoiceNo = Convert.ToString(studentprograminvoice.Id);
            this._studentprograminvoiceRepository.Update(existing);
            return studentprograminvoice.Id;
        }
        public List<StudentProgramInvoiceDataSet> GetInvoiceData(int StatusId, int SubjectId, string StudentName)
        {
            var allData = from a in this._studentprograminvoiceRepository.Table
                          where a.StudentProgram.Student.IsActive == true && a.StudentProgram.IsActive == true && a.PaymentStatus == (StatusId == 0 ? a.PaymentStatus : StatusId) && a.StudentProgram.SubjectId == (SubjectId == 0 ? a.StudentProgram.SubjectId : SubjectId)
                          select new StudentProgramInvoiceDataSet
                          {
                              Id = a.Id,
                              StudentProgramId = a.StudentProgramId,
                              StudentName = a.StudentProgram.Student.FirstName + " " + a.StudentProgram.Student.LastName,
                              PaymentStatus = (a.PaymentStatus == 1) ? "UnPaid" : "Paid",
                              IsPaid = (a.PaymentStatus == 2),
                              Mobile = a.StudentProgram.Student.Mobile,
                              Department = a.StudentProgram.Division.Name,
                              Class = a.StudentProgram.Subject.Name,
                              InvoiceDate = a.InvoiceDate,
                              PaymentDate = a.PaymentDate

                              //ISActive = a.StudentProgram.Student.IsActive,
                          };
            if (!string.IsNullOrEmpty(StudentName))
                allData = allData.Where(w => w.StudentName.Contains(StudentName));

            return allData.ToList();
        }
        public List<RegistrationDataSet> GetStudInvoice(int SubjectId, string StudentName)
        {
            DateTime dtNext = DateTime.UtcNow.Date.AddMonths(1);
            var allData = from a in this._studentProgramRepository.Table
                          where a.Student.IsRegistered == true && a.IsActive == true && a.Student.IsActive == true && a.SubjectId == (SubjectId == 0 ? a.SubjectId : SubjectId)
                          select new RegistrationDataSet
                          {
                              Id = a.Id,
                              StudentId = a.StudentId,
                              StudentName = a.Student.FirstName + " " + a.Student.LastName,
                              PhoneNumber = a.Student.Mobile,
                              Department = a.Division.Name,
                              Class = a.Subject.Name,
                              Section = a.Batch.Name,
                              Active = a.IsActive,
                              NextDueDate = a.NextDueDate
                          };

            if (!string.IsNullOrEmpty(StudentName))
                allData = allData.Where(w => w.StudentName.Contains(StudentName));

            return allData.ToList();
        }
        public void Update(int Id,int PaymentType, string BankName, string IFSCCode, bool IsChequeClear)//int FeeId,
        {
            var existingStudentProgInvoice = this._studentprograminvoiceRepository.GetById(Id);

            if (existingStudentProgInvoice != null)
            {
                existingStudentProgInvoice.PaymentDate = DateTime.UtcNow;
                existingStudentProgInvoice.IsChequeClear = IsChequeClear;
                existingStudentProgInvoice.PaymentType = PaymentType;
                if (existingStudentProgInvoice.PaymentType == 2 && !existingStudentProgInvoice.IsChequeClear)
                    existingStudentProgInvoice.PaymentStatus = 1; //UnPaid
                else
                    existingStudentProgInvoice.PaymentStatus = 2; //Paid
                existingStudentProgInvoice.BankName = BankName;
                existingStudentProgInvoice.IFSCCode = IFSCCode;
                this._studentprograminvoiceRepository.Update(existingStudentProgInvoice);

                if (existingStudentProgInvoice.PaymentType == 2 && !existingStudentProgInvoice.IsChequeClear)
                    return;
                int FeeId =_studentprograminvoiceRepository.Table.Where(w=>w.Id == Id).Select(s=>s.StudentProgram.FeeId).FirstOrDefault();
                var feeFrequency = this._feeRepository.Table.Where(w => w.Id == FeeId).Select(s => s.FeeFrequency).FirstOrDefault();
                var studentProgram = this._studentprograminvoiceRepository.Table.Where(w => w.Id == Id).Select(s => s.StudentProgram).FirstOrDefault();

                studentProgram.NextDueDate = studentProgram.NextDueDate.Date.AddMonths(feeFrequency.Frequency);
                studentProgram.FeeId = FeeId;
                this._studentProgramRepository.Update(studentProgram);
            }
        }
        public Data.Models.StudentProgramInvoice GetForId(int Id)
        {
            return this._studentprograminvoiceRepository.Table.FirstOrDefault(f => f.Id == Id);
        }
        public int DeleteStudentProgramInvoice(int invoiceId)
        {

            var studentprograminvoice = this._studentprograminvoiceRepository.Table.FirstOrDefault(w => w.Id == invoiceId);

            if (studentprograminvoice.PaymentStatus == 2)
                return 101;

            this._studentprograminvoiceRepository.Delete(studentprograminvoice);
            return 0;
        }
        public int DeleteRegisterStudent(int invoiceId)
        {

            var studentprograminvoice = this._studentprograminvoiceRepository.Table.FirstOrDefault(w => w.Id == invoiceId);

            if (studentprograminvoice.PaymentType == 1)
                return 101;

            this._studentprograminvoiceRepository.Delete(studentprograminvoice);
            return 0;
        }
        public List<StudentFeeDataSet> GetFeeInfo(int Id)
        {
            var allData = from a in this._studentprograminvoiceRepository.Table
                          where a.StudentProgram.StudentId == Id
                          select new StudentFeeDataSet
                          {
                              Id = a.Id,
                              InvoiceDate = a.InvoiceDate,
                              PaymentDate = a.PaymentDate,
                              InvoiceAmount = a.InvoiceAmount,
                              SubjectName = a.StudentProgram.Subject.Name,
                              PaymentStatus = (a.PaymentStatus == 1) ? "UnPaid" : "Paid",
                              // ISActive = a.StudentProgram.Student.IsActive,
                          };

            return allData.ToList();
        }
        public List<StudentFeeDataSet> GetInvoiceDetails(int Id)
        {
            var allData = from a in this._studentprograminvoiceRepository.Table
                          where a.StudentProgram.Id == Id
                          select new StudentFeeDataSet
                          {
                              Id = a.Id,
                              StudentProgramId=a.StudentProgramId,
                              InvoiceDate = a.InvoiceDate,
                              PaymentDate = a.PaymentDate,
                              InvoiceAmount = a.InvoiceAmount,
                              SubjectName = a.StudentProgram.Subject.Name,
                              PaymentStatus = (a.PaymentStatus == 1) ? "UnPaid" : "Paid",
                              IsPaid = (a.PaymentStatus == 2),
                              PaymentType = a.PaymentType == null ? "" : (a.PaymentType == 1) ? "Cash" : "Cheque"
                              // ISActive = a.StudentProgram.Student.IsActive,
                          };
            var finalData = allData.OrderByDescending(o => o.InvoiceDate).Take(10).ToArray();

            return finalData.OrderBy(o => o.InvoiceDate).ToList();
        }
        public int GetFrequency(int StudentProgramId, out int Amount)
        {
            var allData = from a in this._studentProgramRepository.Table
                          where a.Id == StudentProgramId
                          select a;

            var s = allData.FirstOrDefault();
            Amount = s.Fee.Amount;
            return s.Fee.FeeFrequency.Frequency;
        }
        public DateTime getLastDate(int StudentProgramId)
        {
            if (!this._studentprograminvoiceRepository.Table.Any(w => w.StudentProgramId == StudentProgramId))
                return DateTime.Now.Date;
            //return this._studentprograminvoiceRepository.Table.Where(w=>w.StudentProgramId == StudentProgramId).Max(s=>s.InvoiceDate);
            return this._studentProgramRepository.Table.Where(w => w.Id == StudentProgramId).Max(s => s.NextDueDate);
        }
        public void UpdatePayStatus(int Id, bool PaymentStatus)
        {
            var existingStudentProgInvoice = this._studentprograminvoiceRepository.GetById(Id);
            
            if (existingStudentProgInvoice != null)
            {
                if (PaymentStatus == true)
                    existingStudentProgInvoice.PaymentStatus = 2;
                else
                    existingStudentProgInvoice.PaymentStatus = 1;

                existingStudentProgInvoice.PaymentDate = null;
                existingStudentProgInvoice.PaymentType = null;
                existingStudentProgInvoice.BankName = null;
                // existingStudentProgInvoice.IsChequeClear = null;
                existingStudentProgInvoice.IFSCCode = null;
                //  existingStudentProgInvoice.PaymentStatus = null;
                var studentProgram = this._studentprograminvoiceRepository.Table.Where(w => w.Id == Id).Select(s => s.StudentProgram).FirstOrDefault();
                studentProgram.NextDueDate = existingStudentProgInvoice.InvoiceDate;

                this._studentProgramRepository.Update(studentProgram);
                this._studentprograminvoiceRepository.Update(existingStudentProgInvoice);
            }
        }

        public bool getAllData(int StudentProgramId)
        {
            int count= this._studentprograminvoiceRepository.Table.Where(w => (w.StudentProgramId == StudentProgramId)).Count();
            int paycount= this._studentprograminvoiceRepository.Table.Where(w => (w.StudentProgramId == StudentProgramId && w.PaymentStatus == 2)).Count();
            return (count == paycount);
        }

        public List<InvoiceDataset> getInvoiceDetails(int Id)
        {
            var invoiceData = this._studentprograminvoiceRepository.Table.FirstOrDefault(w=>w.Id == Id);
            var frequncy = invoiceData.StudentProgram.Fee.FeeFrequency.Frequency;
            var nextinvoice = invoiceData.InvoiceDate.AddMonths(frequncy);
            var endDate = nextinvoice.AddDays(-1).Date;


            var allData = from a in this._studentprograminvoiceRepository.Table
                          where a.Id == Id
                          select new InvoiceDataset
                          {
                              Id = a.Id,
                              InvoiceNo = a.InvoiceNo,
                              StudentProgramId = a.StudentProgramId,
                              StudentId = a.StudentProgram.StudentId,
                              SubjectId = a.StudentProgram.SubjectId,
                              BatchId = a.StudentProgram.BatchId,
                              DivisionId = a.StudentProgram.DivisionId,
                              Frequency = a.StudentProgram.Fee.FeeFrequency.Frequency,
                              FrequencyName = a.StudentProgram.Fee.FeeFrequency.Description,
                              InvoiceAmount = a.InvoiceAmount,
                              InvoiceDate = a.InvoiceDate,
                              PaymentDate = a.PaymentDate,
                              PaymentStatus = a.PaymentStatus,
                              FirstName = a.StudentProgram.Student.FirstName,
                              MiddleName=a.StudentProgram.Student.MiddleName,
                              LastName = a.StudentProgram.Student.LastName,
                              Address = a.StudentProgram.Student.Address,
                              SubjectName = a.StudentProgram.Subject.Name,
                              DivisionName = a.StudentProgram.Division.Name,
                              BatchName = a.StudentProgram.Batch.Name,
                              Mobile = a.StudentProgram.Student.Mobile,
                              Tax = 0,
                              PeriodStartDate = a.PeriodStartDate,
                              PeriodEndDate=a.PeriodEndDate
                          };
            return allData.ToList();
        }

        public List<Data.Models.Setting> InstitutionDetails()
        {
            return this._settingRepository.Table.Where(w => w.Name != "AboutSchool").ToList();
        }

        public void UpdateNextDueDate(int StudentProgramId, DateTime invoicedate,int FeeId)
        {
            var existingStudentProg = this._studentProgramRepository.GetById(StudentProgramId);

            if (existingStudentProg != null)
            {
                existingStudentProg.NextDueDate = invoicedate;
                existingStudentProg.FeeId = FeeId;
                this._studentProgramRepository.Update(existingStudentProg);
            }
        }
    }
}
