using System;
using Microsoft.Extensions.DependencyInjection;

namespace SmartHome.MongoService.BsonCustomSerializers;

public class MongoSerializerConfigurator
{
    private readonly IServiceCollection _services;

    internal  MongoSerializerConfigurator(IServiceCollection services)
    {
        _services = services;
    }
    
    public IServiceCollection ConfigureSerializer(Action<MongoSerializerBuilder> settings)
    {
        settings(new MongoSerializerBuilder());
        return _services;
    }
}