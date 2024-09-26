using SmartHome.Common.Models.Db;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartHome.Common.Interfaces;

public interface ISettingService
{
    public  Task<IEnumerable<Setting>> GetAllSetting();
    public Task<TSetting> GetSetting<TSetting>(string id) where TSetting : Setting, new();

    public TSetting CreateSetting<TSetting>(TSetting setting) where TSetting : Setting, new();
}