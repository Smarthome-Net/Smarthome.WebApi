using System.Text.Json;
using MongoDB.Bson;
using MongoDB.Driver;
using SmartHome.Cli.Tools.Mappers;
using SmartHome.Cli.Tools.Services;
using SmartHome.Common.Models.Db;
using SmartHome.MongoService.DbContext;
using Spectre.Console;
using Spectre.Console.Cli;

namespace SmartHome.Cli.Tools.Commands;

public class InitializeDatabaseCommand : AsyncCommand
{
    private readonly MongDbManagementContext _managementContext;
    private readonly IResourceManager _resourceManager;

    public InitializeDatabaseCommand(MongDbManagementContext managementContext, IResourceManager resourceManager)
    {
        _managementContext = managementContext;
        _resourceManager = resourceManager;
    }

    public override async Task<int> ExecuteAsync(CommandContext context)
    {
        AnsiConsole.WriteLine("Start initializing Database.");
        var missingCollections = await GetMissingCollections();
        if (missingCollections.Length == 0)
        {
            AnsiConsole.WriteLine("Database already initialized.");
            return 0;
        }

        var temperatureCollectionOptions = new CreateCollectionOptions
        {
            TimeSeriesOptions = new TimeSeriesOptions(nameof(Temperature.RecordDateTime))
        };

        await CreateCollection(missingCollections, Collection.Device);
        await CreateCollection(missingCollections, Collection.Setting);
        await CreateCollection(missingCollections, Collection.Temperature, temperatureCollectionOptions);

        var settingCollection = _managementContext.Database.GetCollection<BsonDocument>(Collection.Setting);
        if (await settingCollection.EstimatedDocumentCountAsync() > 0)
        {
            AnsiConsole.WriteLine("Setting already imported.");
            return 0;
        }

        AnsiConsole.WriteLine("Load default setting into database.");
        var defaultSettings = await _resourceManager.GetJsonResource<Dictionary<string, JsonElement>[]>("smarthome.setting.json") ?? [];
        var bsonDocuments = defaultSettings.AsBsonDocuments();
        await settingCollection.InsertManyAsync(bsonDocuments);

        AnsiConsole.WriteLine("Database initialized successfuly.");
        return 0;
    }

    private async Task CreateCollection(string[] missingCollections, string collection, CreateCollectionOptions? options = null)
    {
        if (missingCollections.Contains(collection))
        {
            AnsiConsole.WriteLine($"Create collection: {collection}");
            await _managementContext.Database.CreateCollectionAsync(collection, options);
        }
    }

    private async Task<string[]> GetMissingCollections()
    {
        string[] values = [Collection.Setting, Collection.Device, Collection.Temperature];
        var filter = Builders<BsonDocument>.Filter.In("name", values);
        var cursor = await _managementContext.Database.ListCollectionsAsync(new ListCollectionsOptions
        {
            Filter = filter
        });
        var existingCollections = await cursor.ToListAsync();
        return [.. values.Where(name => !existingCollections.Any(d => string.Equals(d["name"].AsString, name)))];
    }
}
