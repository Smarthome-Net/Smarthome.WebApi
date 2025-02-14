using System.Reflection;
using System.Text.Json;

namespace SmartHome.Cli.Tools.Services;

public class RessourceManager : IRessourceManager
{
    private readonly Assembly _assembly;

    public RessourceManager()
    {
        _assembly = Assembly.GetExecutingAssembly();
    }

    public async Task<TType?> GetJsonRessource<TType>(string path)
    {
        var fullPath = _assembly.GetManifestResourceNames().FirstOrDefault(name => name.EndsWith(path));
        var stream = _assembly.GetManifestResourceStream(fullPath!) ?? throw new InvalidOperationException($"Ressource not found: {path}");
        return await JsonSerializer.DeserializeAsync<TType>(stream);
    }
}
