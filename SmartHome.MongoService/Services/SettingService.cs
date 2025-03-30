using MongoDB.Driver;
using SmartHome.Common.Interfaces;
using SmartHome.Common.Models.Db;
using SmartHome.MongoService.DbContext;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartHome.MongoService.Services;

public class SettingService : ISettingService
{
    private readonly MongoDbContext _mongoDbContext;
    
    public SettingService(MongoDbContext mongoDbContext)
    {
        _mongoDbContext = mongoDbContext; 
    }

    public async Task<long> UpdateSetting<TSetting>(TSetting setting, UpdateDefinition<TSetting> updateDefinition) where TSetting : Setting, new()
    {
        var filter = Builders<TSetting>.Filter.Eq(s => s.Id, setting.Id);
        var result =  await _mongoDbContext.SettingCollection
            .OfType<TSetting>()
            .UpdateOneAsync(filter, updateDefinition);
        return result.IsAcknowledged ? result.ModifiedCount : 0;
    }

    public async Task<IEnumerable<Setting>> GetAllSetting()
    {
        var cursor = await _mongoDbContext.SettingCollection.FindAsync(a => true);
        return await cursor.ToListAsync();
    }

    public async Task<TSetting> GetSetting<TSetting>() where TSetting : Setting, new()
    { 
       var cursor = await _mongoDbContext.SettingCollection
            .OfType<TSetting>()
            .FindAsync(a => true);
       return await cursor.FirstOrDefaultAsync();
    }
}
