using SmartHome.Common.Models.Db;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace SmartHome.Common.Interfaces;

public interface ISettingService
{
    Task<IEnumerable<Setting>> GetAllSetting();
    Task<TSetting> GetSetting<TSetting>() where TSetting : Setting, new();
    Task<long> UpdateSetting<TSetting>(TSetting setting, UpdateDefinition<TSetting> updateDefinition) where TSetting : Setting, new();
}