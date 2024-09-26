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
    public readonly MongoDBContext _mongoDBContext;
    
    public SettingService(MongoDBContext mongoDBContext)
    {
        _mongoDBContext = mongoDBContext; 
    }

    public TSetting CreateSetting<TSetting>(TSetting setting) where TSetting : Setting, new()
    {
        _mongoDBContext.SettingCollection!.InsertOne(setting);
        return setting;
    }

    public async Task<IEnumerable<Setting>> GetAllSetting()
    {
        var result = await _mongoDBContext.SettingCollection.FindAsync(a => true);
        return await result.ToListAsync();
    }

    public Task<TSetting> GetSetting<TSetting>(string id) where TSetting : Setting, new()
    {
        var filter = Builders<Setting>.Filter.Eq(a => a.Id, ObjectId.Parse(id));
        var projection = Builders<Setting>.Projection.As<TSetting>();
        var result = _mongoDBContext.SettingCollection
            .Find(filter)
            .Project(projection);
        return result.FirstOrDefaultAsync();
    }
}
