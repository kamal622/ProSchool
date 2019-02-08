using ProSchool.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProSchool.Services.Academics
{
    public class StudentProgramService
    {
        private readonly IRepository<Data.Models.StudentProgram> _studentProgramRepository;
        private readonly IRepository<Data.Models.StudentProgramInvoice> _studentProgramInvoiceRepository;
        public StudentProgramService(IRepository<Data.Models.StudentProgram> studentProgramRepository, IRepository<Data.Models.StudentProgramInvoice> studentProgramInvoiceRepository)
        {
            this._studentProgramRepository = studentProgramRepository;
            this._studentProgramInvoiceRepository = studentProgramInvoiceRepository;
        }
        public int Insert(Data.Models.StudentProgram StudentProgram, out int errorCode)
        {
            errorCode = 0;
            if (this._studentProgramRepository.Table.Any(a => a.StudentId == StudentProgram.StudentId && a.SubjectId == StudentProgram.SubjectId && a.DivisionId == StudentProgram.DivisionId))
            {
                errorCode = 101;
                return 0;
            }

            this._studentProgramRepository.Insert(StudentProgram);
            return StudentProgram.Id;
        }
        public void Update(int Id, bool Checked)
        {
            var existingStudentProg = this._studentProgramRepository.GetById(Id);

            if (existingStudentProg != null)
            {
                existingStudentProg.IsActive = Checked;
                this._studentProgramRepository.Update(existingStudentProg);
            }
        }
        public void UpdateNextDueDate(int Id, DateTime extendDate)
        {
            var existingStudentProg = this._studentProgramRepository.GetById(Id);

            if (existingStudentProg != null)
            {
                existingStudentProg.NextDueDate = extendDate;
                this._studentProgramRepository.Update(existingStudentProg);
            }
        }
        public Data.Models.StudentProgram GetForId(int Id)
        {
            return this._studentProgramRepository.Table.FirstOrDefault(f => f.StudentId == Id);
        }
        public Data.Models.StudentProgram GetProgramId(int Id)
        {
            return this._studentProgramRepository.Table.FirstOrDefault(f => f.Id == Id);
        }

        public DateTime[] GetDueDates()
        {
            return this._studentProgramRepository.Table.Where(w => w.IsActive == true && w.Student.IsActive == true).Select(s => s.NextDueDate).ToArray();
        }
        public int UpdateStudProgram(Data.Models.StudentProgram studentprogram, out int errorCode)
        {
            errorCode = 0;
            if (this._studentProgramRepository.Table.Any(a => a.StudentId == studentprogram.StudentId && a.DivisionId == studentprogram.DivisionId && a.SubjectId == studentprogram.SubjectId && a.Id == studentprogram.Id))
            {
                errorCode = 101;
                return 0;
            }

            Data.Models.StudentProgram existingProgram = _studentProgramRepository.Table.FirstOrDefault(w => w.Id == studentprogram.Id);

            if (existingProgram != null)
            {
                existingProgram.SubjectId = studentprogram.SubjectId;
                existingProgram.DivisionId = studentprogram.DivisionId;
                existingProgram.FeeId = studentprogram.FeeId;
                existingProgram.BatchId = studentprogram.BatchId;
                _studentProgramRepository.Update(existingProgram);

                return existingProgram.Id;
            }
            else
                return 0;
        }
    }
}
