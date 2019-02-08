using ProSchool.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProSchool.Services.Academics
{
    public class InquiryService
    {
        private readonly IRepository<Data.Models.Inquiry> _inquiryRepository;

        public InquiryService(IRepository<Data.Models.Inquiry> inquiryRepository)
        {
            this._inquiryRepository = inquiryRepository;

        }
        public int Insert(Data.Models.Inquiry inquiry)
        {
            this._inquiryRepository.Insert(inquiry);
            return inquiry.Id;
        }
        public Data.Models.Inquiry GetForId(int Id)
        {
            return this._inquiryRepository.Table.FirstOrDefault(f => f.Id == Id);
        }
    }
}
