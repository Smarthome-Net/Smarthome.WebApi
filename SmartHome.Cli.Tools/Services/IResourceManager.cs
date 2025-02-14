using System.Text.Json;

namespace SmartHome.Cli.Tools.Services;

public interface IResourceManager
{
    Task<TType?> GetJsonResource<TType>(string path);
}
