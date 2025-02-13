using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using SmartHome.Cli.Tools.Models;
using SmartHome.Common.Models.Db;
using SmartHome.MongoService.DbContext;
using Spectre.Console;
using Spectre.Console.Cli;

namespace SmartHome.Cli.Tools.Commands;

public class MigrateTempartureValuesCommand : Command
{
    private readonly MongDbManagementContext _context;

    public MigrateTempartureValuesCommand(MongDbManagementContext context)
    {
        _context = context;
    }

    public override int Execute(CommandContext context)
    {
        AnsiConsole.WriteLine("Start migration of datatime values from temperature values");
        if (IsCollectionMigrated())
        {
            AnsiConsole.WriteLine("Collection already migrated, no work necessary");
            return 1;
        }
        
        var sourceCollection = _context.Database.GetCollection<Temperature>(Collection.Temperature);
        var result = sourceCollection.Find(v => true);
        var values = result.ToList();
        AnsiConsole.WriteLine($"Loaded {values.Count} temperature values");

        AnsiConsole.MarkupLine($"[red] Drop existing database {Collection.Temperature}[/]");
        _context.Database.DropCollection(Collection.Temperature);

        AnsiConsole.MarkupLine($"[green] Recreate database {Collection.Temperature} as time series[/]");
        _context.Database.CreateCollection(Collection.Temperature, new CreateCollectionOptions
        {
            TimeSeriesOptions = new TimeSeriesOptions(nameof(TemperatureNext.RecordDateTime)),
        });
        var targetCollection = _context.Database.GetCollection<TemperatureNext>(Collection.Temperature);

        AnsiConsole.WriteLine($"Inserting data into recreated collection...");
        targetCollection.InsertMany(values.Select(v => new TemperatureNext
        {
            Value = v.Value,
            RecordDateTime = v.RecordDateTime,
            DeviceId = v.DeviceId,
            Id = v.Id,
        }));
        var estimatedDocumentCount = targetCollection.EstimatedDocumentCount();
        AnsiConsole.WriteLine($"Migrated {values.Count}/{estimatedDocumentCount} datetime values in temperature collection");
        return 0;
    }

    private bool IsCollectionMigrated()
    {
        var nameFiler = Builders<BsonDocument>.Filter.Eq(f => f["name"], Collection.Temperature);
        var typeFilter = Builders<BsonDocument>.Filter.Eq(f => f["type"], "timeseries");
        var filter = Builders<BsonDocument>.Filter.And(nameFiler, typeFilter);
        var collections = _context.Database.ListCollections(options: new ListCollectionsOptions
        {
            Filter = filter
        });
        var isMigrated = collections.Any();
        return isMigrated;
    }
}
