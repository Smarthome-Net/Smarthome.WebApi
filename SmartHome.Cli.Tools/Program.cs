using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartHome.Cli.Tools.Commands;
using SmartHome.Cli.Tools.Infrastructure;
using SmartHome.Cli.Tools.Services;
using SmartHome.MongoService.Extension;
using SmartHome.MongoService.Settings;
using Spectre.Console.Cli;

var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true)
    .Build();

var services = new ServiceCollection();
services.AddMongoDbService(o =>
{
    var connectionSetting = config.GetSection("DbConnectionSetting").Get<DbConnectionSetting>()!;
    o.DbConnectionSetting = connectionSetting;
});
services.AddLogging();
services.AddScoped<IRessourceManager, RessourceManager>();


var registrar = new TypeRegistrar(services);
var app = new CommandApp(registrar);
app.Configure(conf =>
{
    conf.AddBranch("migrate", a =>
    {
        a.AddCommand<MigrateTempartureValuesCommand>("temperature");
    });
    conf.AddCommand<InitializeDatabaseCommand>("initDb");  
    conf.PropagateExceptions();
});

app.Run(args);
