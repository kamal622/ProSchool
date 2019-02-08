using ProSchool.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProSchool.Services.Settings
{
  public class SystemService
    {
        private readonly IRepository<Data.Models.Setting> _settingRepository;
        public SystemService(IRepository<Data.Models.Setting> settingRepository)
        {
            this._settingRepository = settingRepository;
        }
        public int Insert(Data.Models.Setting setting)
        {
            this._settingRepository.Insert(setting);
            return setting.Id;
        }
        public string GetValueForName(string name)
        {
            var setting = this._settingRepository.Table.FirstOrDefault(f => f.Name == name);
            if (setting == null)
                return "";
            return setting.Value;
        }
        public void UpdateSystem(Data.Models.Setting[] system)
        {
            foreach (var value in system)
            {
                var existingItem = this._settingRepository.Table.FirstOrDefault(a => a.Name == value.Name);
                if (existingItem == null)
                {
                    //Insert
                    this._settingRepository.Insert(value);
                }
                else
                {
                    //Update
                    existingItem.Value = value.Value;
                    this._settingRepository.Update(existingItem);
                }
            }
        }

    }
}
