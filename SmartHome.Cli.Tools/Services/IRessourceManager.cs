using System.Text.Json;

namespace SmartHome.Cli.Tools.Services;

public interface IRessourceManager
{
    Task<TType?> GetJsonRessource<TType>(string path);
}
