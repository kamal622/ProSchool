using ProSchool.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProSchool.Services.Settings
{
    public class InfrastructureService
    {
        private readonly IRepository<Data.Models.Division> _divisonRepository;
        private readonly IRepository<Data.Models.StudentProgram> _studentPrpgramRepository;
        public InfrastructureService(IRepository<Data.Models.Division> divisonRepository, 
            IRepository<Data.Models.StudentProgram> studentPrpgramRepository)
        {
            this._divisonRepository = divisonRepository;
            this._studentPrpgramRepository = studentPrpgramRepository;
        }

        public List<Data.Models.Division> GetAll()
        {
            return this._divisonRepository.Table.ToList();
        }

        public int Insert(Data.Models.Division division)
        {
            this._divisonRepository.Insert(division);
            return division.Id;
        }

        public int Update(int Id, string name, string desc) //void
        {
            var existingDivision = this._divisonRepository.GetById(Id);

            if (existingDivision != null)
            {
                if (this._divisonRepository.Table.Any(a => a.Name == name))
                {
                    return 101;
                }
                existingDivision.Name = name;
                existingDivision.Description = desc;
                this._divisonRepository.Update(existingDivision);
            }
            return 0;
        }

        public int Delete(int Id) //void
        {
            var existingDivision = this._divisonRepository.GetById(Id);
            var existingStudentProgram =_studentPrpgramRepository.Table.FirstOrDefault(w=>w.DivisionId == Id);
            if(existingStudentProgram != null)
            {
                return 101;
            }
            this._divisonRepository.Delete(existingDivision);
            return 0;
        }

        public bool IsDivisionNameExists(string name)
        {
            return this._divisonRepository.Table.Any(a=>a.Name == name);
        }
    }
}
