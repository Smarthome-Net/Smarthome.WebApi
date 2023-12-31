﻿namespace SmartHome.MongoService.Settings;

public class DbConnectionSetting
{
    public string Host { get; set; }
    public int Port { get; set; }
    public string User { get; set; }
    public string Password { get; set; }
    public string Database { get; set; }

    public string GetMongoConnectionString()
    {
        return $@"mongodb://{User}:{Password}@{Host}:{Port}/{Database}";
    }
}
