using ProSchool.Core.Data;
using ProSchool.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProSchool.Services.Academics
{
    public class StudentService
    {
        private readonly IRepository<Data.Models.Student> _studentRepository;
        private readonly IRepository<Data.Models.Inquiry> _inquiryRepository;
        private readonly IRepository<Data.Models.StudentProgram> _studentProgramRepository;
        private readonly IRepository<Data.Models.StudentProgramInvoice> _studentProgramInvoiceRepository;

        public StudentService(IRepository<Data.Models.Student> studentRepository, IRepository<Data.Models.Inquiry> inquiryRepository,
            IRepository<Data.Models.StudentProgram> studentProgramRepository, IRepository<Data.Models.StudentProgramInvoice> studentProgramInvoiceRepository)
        {
            this._studentRepository = studentRepository;
            this._inquiryRepository = inquiryRepository;
            this._studentProgramRepository = studentProgramRepository;
            this._studentProgramInvoiceRepository = studentProgramInvoiceRepository;
        }
        public int Insert(Data.Models.Student student)
        {
            this._studentRepository.Insert(student);
            return student.Id;
        }
        public List<Data.Models.Student> GetAllStudent(int registerStatusId)
        {
            return this._studentRepository.Table.Where(s => s.IsRegistered == (registerStatusId == 1 ? true : registerStatusId == 2 ? false : s.IsRegistered)).ToList();
        }

        public List<RegistrationDataSet> GetAllRegistrations(int DivisionId, int BatchId, int SubjectId)
        {
            var allData = from a in this._studentProgramRepository.Table
                          where a.Student.IsRegistered == true
                          && a.DivisionId == (DivisionId == 0 ? a.DivisionId : DivisionId) && a.BatchId == (BatchId == 0 ? a.BatchId : BatchId) && a.SubjectId == (SubjectId == 0 ? a.SubjectId : SubjectId)
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

            return allData.ToList();
        }
        public List<RegistrationDataSet> GetDashboardData()
        {
            DateTime dtNext = DateTime.UtcNow.Date.AddDays(15); //AddMonths
            var allData = from a in this._studentProgramRepository.Table
                          where a.Student.IsRegistered == true && a.IsActive == true && a.Student.IsActive == true && a.NextDueDate <= dtNext
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
                              NextDueDate = a.NextDueDate,
                              //Status = (a.PaymentStatus==1) ? "UnPaid" : "Paid",
                          };
            return allData.ToList();
        }
        public Data.Models.Student GetForEditId(int Id)
        {
            return this._studentRepository.Table.FirstOrDefault(f => f.Id == Id );
        }
        public Data.Models.Student GetForId(int Id)
        {
            return this._studentRepository.Table.FirstOrDefault(f => f.Id == Id);
        }
        public int Update(Data.Models.Student student)
        {
            Data.Models.Student existingStudent = _studentRepository.Table.FirstOrDefault(w => w.Id == student.Id);

            if (existingStudent != null)
            {
                existingStudent.FirstName = student.FirstName;
                existingStudent.MiddleName = student.MiddleName;
                existingStudent.LastName = student.LastName;
                existingStudent.Gender = student.Gender;
                existingStudent.Address = student.Address;
                existingStudent.Mobile = student.Mobile;
                existingStudent.DOB = student.DOB;
                existingStudent.Email = student.Email;
                existingStudent.Remarks = student.Remarks;
                existingStudent.School = student.School;
                existingStudent.College = student.College;
                existingStudent.SCAddress = student.SCAddress;
                existingStudent.ContactName = student.ContactName;
                existingStudent.ContactNumber = student.ContactNumber;
                existingStudent.ContactRelation = student.ContactRelation;
                existingStudent.OriginalFileName = student.OriginalFileName;
                existingStudent.SystemFileName = student.SystemFileName;
                existingStudent.RegistrationNo = student.RegistrationNo;
                existingStudent.IsRegistered = student.IsRegistered;

                _studentRepository.Update(existingStudent);

                return existingStudent.Id;
            }
            else
                return 0;
        }
        public int UpdateInquiry(Data.Models.Student student, Data.Models.Inquiry inquiry)
        {
            Data.Models.Student existingStudent = _studentRepository.Table.FirstOrDefault(w => w.Id == student.Id);

            if (existingStudent != null)
            {
                existingStudent.FirstName = student.FirstName;
                existingStudent.MiddleName = student.MiddleName;
                existingStudent.LastName = student.LastName;
                existingStudent.Gender = student.Gender;
                existingStudent.Address = student.Address;
                existingStudent.Mobile = student.Mobile;
                existingStudent.DOB = student.DOB;
                existingStudent.Email = student.Email;
                existingStudent.Remarks = student.Remarks;
                _studentRepository.Update(existingStudent);

                return existingStudent.Id;
            }
            else
                return 0;
        }
        public int DeleteInquiry(int studentId)
        {
            //var deletedItem = this._inquiryRepository.Table.Where(w => w.Id==studentId);
            //this._studentRepository.Delete(this._studentRepository.Table.Where(w => w.InquiryId==studentId));
            //this._inquiryRepository.Delete(deletedItem);

            var student = this._studentRepository.Table.FirstOrDefault(w => w.Id == studentId);

            if (student.IsRegistered)
                return 101;

            this._studentRepository.Delete(student);
            this._inquiryRepository.Delete(this._inquiryRepository.Table.Where(w => w.Id == student.InquiryId));
            return 0;
        }
        public int DeleteRegStudentByID(int Id)
        {
           var student = this._studentProgramRepository.Table.FirstOrDefault(w => w.Id == Id);
           var studentInvoice = this._studentProgramInvoiceRepository.Table.FirstOrDefault(w => w.StudentProgramId == Id);
           if (studentInvoice != null)
                return 101;

            var existingStudent = _studentRepository.GetById(student.StudentId);
            if(existingStudent != null)
            {
                existingStudent.IsRegistered = false;
            }
           this._studentRepository.Update(existingStudent);
           this._studentProgramRepository.Delete(student);
           return 0;
        }
        public List<StudentDataSet> GetStudentData(int DivisionId, int BatchId, int SubjectId)
        {
            var allData = this._studentRepository.Table.Where(w => w.StudentPrograms.Any(a => a.DivisionId == (DivisionId == 0 ? a.DivisionId : DivisionId) && a.BatchId == (BatchId == 0 ? a.BatchId : BatchId) && a.SubjectId == (SubjectId == 0 ? a.SubjectId : SubjectId)));

            var studentData = allData.Select(s => new { Student = s, StudentPrograms = s.StudentPrograms.Select(a => a.Subject.Name) }).ToArray();

            var finalData = from a in studentData
                            where a.Student.IsRegistered == true
                            select new StudentDataSet
                            {
                                Id = a.Student.Id,
                                StudentName = a.Student.FirstName + " " + a.Student.LastName,
                                Mobile = a.Student.Mobile,
                                IsActive = a.Student.IsActive,
                                Programs = string.Join(",", a.StudentPrograms)
                            };

            return finalData.ToList();
        }
        public void UpdateIsActive(int Id, bool Checked)
        {
            var existingStudent = this._studentRepository.GetById(Id);

            if (existingStudent != null)
            {
                existingStudent.IsActive = Checked;
                this._studentRepository.Update(existingStudent);
            }
        }

        public bool[] GetStudentActiveStatus()
        {
            return this._studentRepository.Table.Select(s => s.IsActive).ToArray();
        }
        public List<Data.Models.Student> GetStudFname()
        {
            return this._studentRepository.Table.OrderBy(s=>s.FirstName).ToList();
        }
    }
}
