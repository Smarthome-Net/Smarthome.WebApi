using Spectre.Console.Cli;

namespace SmartHome.Cli.Tools.Infrastructure;

public class TypeResolver : ITypeResolver
{
    private readonly IServiceProvider _serviceProvider;

    public TypeResolver(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    public object? Resolve(Type? type)
    {
        return _serviceProvider.GetService(type!);
    }
}
