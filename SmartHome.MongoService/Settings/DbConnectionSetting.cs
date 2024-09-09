namespace SmartHome.MongoService.Settings;

public class DbConnectionSetting
{
    public required string Host { get; set; }
    public required int Port { get; set; }
    public required string User { get; set; }
    public required string Password { get; set; }
    public required string Database { get; set; }

    public string GetMongoConnectionString()
    {
        return $@"mongodb://{User}:{Password}@{Host}:{Port}/{Database}";
    }
}
