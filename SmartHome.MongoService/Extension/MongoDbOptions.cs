using SmartHome.MongoService.Settings;

namespace SmartHome.MongoService.Extension;

public class MongoDbOptions
{
    public required DbConnectionSetting DbConnectionSetting { get; set; }
}
