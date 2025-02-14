using MongoDB.Bson;
using MongoDB.Driver;
using SmartHome.Cli.Tools.Models;
using SmartHome.Common.Models.Db;
using SmartHome.MongoService.DbContext;
using Spectre.Console;
using Spectre.Console.Cli;

namespace SmartHome.Cli.Tools.Commands;

public class MigrateTempartureValuesCommand : AsyncCommand
{
    private readonly MongDbManagementContext _context;

    public MigrateTempartureValuesCommand(MongDbManagementContext context)
    {
        _context = context;
    }

    public override async Task<int> ExecuteAsync(CommandContext context)
    {
        AnsiConsole.WriteLine("Start migration of datatime values from temperature values.");
        if (await IsCollectionMigrated())
        {
            AnsiConsole.WriteLine("Collection already migrated, no work necessary.");
            return 1;
        }
        
        var sourceCollection = _context.Database.GetCollection<Temperature>(Collection.Temperature);
        var result = sourceCollection.Find(v => true);
        var values = result.ToList();
        AnsiConsole.WriteLine($"Loaded {values.Count} temperature values.");

        AnsiConsole.MarkupLine($"[red] Drop existing database {Collection.Temperature}.[/]");
        await _context.Database.DropCollectionAsync(Collection.Temperature);

        AnsiConsole.MarkupLine($"[green] Recreate database {Collection.Temperature} as timeseries.[/]");
        await _context.Database.CreateCollectionAsync(Collection.Temperature, new CreateCollectionOptions
        {
            TimeSeriesOptions = new TimeSeriesOptions(nameof(Temperature.RecordDateTime)),
        });
        var targetCollection = _context.Database.GetCollection<TemperatureNext>(Collection.Temperature);

        AnsiConsole.WriteLine($"Inserting data into recreated collection...");
        await targetCollection.InsertManyAsync(values.Select(v => new TemperatureNext
        {
            Value = v.Value,
            RecordDateTime = v.RecordDateTime,
            DeviceId = v.DeviceId,
            Id = v.Id,
        }));
        var estimatedDocumentCount = await targetCollection.EstimatedDocumentCountAsync();
        AnsiConsole.WriteLine($"Migrated {values.Count}/{estimatedDocumentCount} datetime values in temperature collection.");
        return 0;
    }

    private async Task<bool> IsCollectionMigrated()
    {
        //check the type of the temperature collection, if we found one, than the collection is already migrated to a time series collection
        var nameFiler = Builders<BsonDocument>.Filter.Eq(f => f["name"], Collection.Temperature);
        var typeFilter = Builders<BsonDocument>.Filter.Eq(f => f["type"], "timeseries");
        var filter = Builders<BsonDocument>.Filter.And(nameFiler, typeFilter);
        var collections = await _context.Database.ListCollectionsAsync(options: new ListCollectionsOptions
        {
            Filter = filter
        });
        return collections.Any();
    }
}
