using ProSchool.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProSchool.Services.Settings
{
  public  class InstitutionService
    {
        private readonly IRepository<Data.Models.Subject> _subjectRepository;
        private readonly IRepository<Data.Models.Batch> _batchRepository;
        private readonly IRepository<Data.Models.Student> _studentRepository;
        private readonly IRepository<Data.Models.StudentProgram> _studentProgramRepository;
        public InstitutionService(IRepository<Data.Models.Subject> subjectRepository, IRepository<Data.Models.Batch> batchRepository , 
            IRepository<Data.Models.Student> studentRepository, IRepository<Data.Models.StudentProgram> studentProgramRepository)
        {
            this._subjectRepository = subjectRepository;
            this._batchRepository = batchRepository;
            this._studentRepository = studentRepository;
            this._studentProgramRepository = studentProgramRepository;
        }
        //Subject
        public int Insert(Data.Models.Subject subject)
        {
            this._subjectRepository.Insert(subject);
            return subject.Id;
        }
        public bool IsSubjectNameExists(string name)
        {
            return this._subjectRepository.Table.Any(a => a.Name == name);
        }
        public int Update(int Id, string name, string desc) //void
        {
            var existingSubject = this._subjectRepository.GetById(Id);

            if (existingSubject != null)
            {
                if(this._subjectRepository.Table.Any(a=>a.Name == name))
                {
                    return 101;
                }
                existingSubject.Name = name;
                existingSubject.Description = desc;
                this._subjectRepository.Update(existingSubject);
            }
            return 0;
        }
        public List<Data.Models.Subject> GetAll()
        {
            return this._subjectRepository.Table.ToList();
        }
        public List<Data.Models.Subject> GetSub()
        {
            return this._subjectRepository.Table.ToList();
        }
        public int Delete(int Id)//void
        {
            var existingSubject = this._subjectRepository.GetById(Id);
            var existingStudentProgram = _studentProgramRepository.Table.FirstOrDefault(w => w.SubjectId == Id);
            if(existingStudentProgram != null)
            {
                return 101;
            }
            this._subjectRepository.Delete(existingSubject);
            return 0;
        }
        //Batches
        public int Insert(Data.Models.Batch batche)
        {
            this._batchRepository.Insert(batche);
            return batche.Id;
        }
        public bool IsBatcheNameExists(string name)
        {
            return this._batchRepository.Table.Any(a => a.Name == name);
        }
        public int UpdateBatche(int Id, string name, int capacity, string desc)//void
        {
            var existingBatche = this._batchRepository.GetById(Id);

            if (existingBatche != null)
            {
                if(this._batchRepository.Table.Any(a=>a.Name == name))
                {
                    return 101;
                }
                existingBatche.Name = name;
                //existingBatche.SubjectId = subjectId;
                existingBatche.Capacity = capacity;
                existingBatche.Description = desc;
                this._batchRepository.Update(existingBatche);
            }
            return 0;
        }
        public List<Data.Models.Batch> GetAllBatche()
        {
            return this._batchRepository.Table.ToList();
        }
        public int DeleteBatche(int Id) //void
        {
            var existingBatche = this._batchRepository.GetById(Id);
            var existingProgram = _studentProgramRepository.Table.FirstOrDefault(w => w.BatchId == Id);
            if(existingProgram != null)
            {
                return 101;
            }
            this._batchRepository.Delete(existingBatche);
            return 0;
        }
    }
}
