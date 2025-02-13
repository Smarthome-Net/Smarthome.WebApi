using MongoDB.Driver;
using SmartHome.Cli.Tools.Models;
using SmartHome.Common.Models.Db;
using SmartHome.MongoService.DbContext;
using Spectre.Console;
using Spectre.Console.Cli;

namespace SmartHome.Cli.Tools.Commands;

public class MigrateTempartureValuesCommand : Command
{
    private readonly MongDBManagementContext _context;

    public MigrateTempartureValuesCommand(MongDBManagementContext context)
    {
        _context = context;
    }

    public override int Execute(CommandContext context)
    {
        AnsiConsole.WriteLine("Start migration of datatime values from temperature values");
        var sourceCollection = _context.Database.GetCollection<Temperature>(Collection.Temperature);
        var result = sourceCollection.Find(v => true);
        var values = result.ToList();
        AnsiConsole.WriteLine($"Loaded {values.Count} temperature values");

        AnsiConsole.MarkupLine($"[red] Drop existing database {Collection.Temperature}[/]");
        _context.Database.DropCollection(Collection.Temperature);

        AnsiConsole.MarkupLine($"[gren] Recreate database {Collection.Temperature} as time series[/]");
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
}
