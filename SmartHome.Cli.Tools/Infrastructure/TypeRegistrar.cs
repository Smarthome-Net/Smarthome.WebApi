using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace SmartHome.Cli.Tools.Infrastructure;

public class TypeRegistrar : ITypeRegistrar
{
    private readonly IServiceCollection _services;
    public TypeRegistrar(IServiceCollection services)
    {
        _services = services;
    }

    public ITypeResolver Build()
    {
        var serviceProvider = _services.BuildServiceProvider();
        return new TypeResolver(serviceProvider);
    }

    public void Register(Type service, Type implementation)
    {
        _services.AddScoped(service, implementation);
    }

    public void RegisterInstance(Type service, object implementation)
    {
        _services.AddSingleton(service, implementation);
    }

    public void RegisterLazy(Type service, Func<object> factory)
    {
        _services.AddScoped(service, sp => factory());
    }
}