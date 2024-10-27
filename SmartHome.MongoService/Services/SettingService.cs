using MongoDB.Bson;
using MongoDB.Driver;
using SmartHome.Common.Interfaces;
using SmartHome.Common.Models.Db;
using SmartHome.MongoService.DbContext;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartHome.MongoService.Services;

public class SettingService : ISettingService
{
    private readonly MongoDBContext _mongoDBContext;
    
    public SettingService(MongoDBContext mongoDbContext)
    {
        _mongoDBContext = mongoDbContext; 
    }

    public async Task<long> UpdateSetting<TSetting>(TSetting setting, UpdateDefinition<TSetting> updateDefinition) where TSetting : Setting, new()
    {
        var filter = Builders<TSetting>.Filter.Eq(s => s.Id, setting.Id);
        var result =  await _mongoDBContext.SettingCollection!
            .OfType<TSetting>()
            .UpdateOneAsync(filter, updateDefinition);
        return result.IsAcknowledged ? result.ModifiedCount : 0;
    }

    public async Task<IEnumerable<Setting>> GetAllSetting()
    {
        var result = await _mongoDBContext.SettingCollection!.FindAsync(a => true);
        return await result.ToListAsync();
    }

    public async Task<TSetting> GetSetting<TSetting>() where TSetting : Setting, new()
    {
        var projection = Builders<Setting>
            .Projection
            .As<TSetting>();
        var result = await  _mongoDBContext.SettingCollection!
            .Aggregate()
            .Match(a => a is TSetting)
            .Project(projection)
            .FirstOrDefaultAsync();

        return result;
    }
}
