using ProSchool.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProSchool.Services.Finance
{
    public class FeeService
    {
        private readonly IRepository<Data.Models.Fee> _feeRepository;
        private readonly IRepository<Data.Models.FeeFrequency> _feeFrequencyRepository;
        private readonly IRepository<Data.Models.StudentProgram> _studentProgramRepository;
        public FeeService(IRepository<Data.Models.Fee> feeRepository, IRepository<Data.Models.FeeFrequency> feeFrequencyRepository,
            IRepository<Data.Models.StudentProgram> studentProgramRepository)
        {
            this._feeRepository = feeRepository;
            this._feeFrequencyRepository = feeFrequencyRepository;
            this._studentProgramRepository= studentProgramRepository;
        }
        public List<Data.Models.FeeFrequency> GetAllFrequency()
        {
            return this._feeFrequencyRepository.Table.ToList();
        }
        public int Insert(Data.Models.Fee fee)
        {
            this._feeRepository.Insert(fee);
            return fee.Id;
        }
        public List<Data.Models.Fee> GetAllFee()
        {
            return this._feeRepository.Table.OrderBy(o=>o.Name).ToList();
        }
        public Data.Models.Fee GetForId(int Id)
        {
            return this._feeRepository.GetById(Id);
        }
        
        public int Update(Data.Models.Fee fee)
        {
            Data.Models.Fee existingFee = _feeRepository.Table.FirstOrDefault(w => w.Id == fee.Id);

            if (existingFee != null)
            {
                existingFee.Name = fee.Name;
                existingFee.Frequency = fee.Frequency;
                existingFee.Amount = fee.Amount;

                _feeRepository.Update(existingFee);

                return existingFee.Id;
            }
            else
                return 0;
        }
        public int Delete(int Id)//void
        {
            var existingFee = this._feeRepository.GetById(Id);
            var existingStudentProgram = this._studentProgramRepository.Table.FirstOrDefault(w => w.FeeId == Id);
            if(existingStudentProgram != null)
            {
                return 101;
            }
            this._feeRepository.Delete(existingFee);
            return 0;
        }

        public Data.Models.Fee GetFrequencyId(int FeeId)
        {
            return this._feeRepository.Table.FirstOrDefault(w => w.Id == FeeId);
        }
        public bool IsFeeNameExists(string name)
        {
            return this._feeRepository.Table.Any(a => a.Name == name);
        }
    }
}
