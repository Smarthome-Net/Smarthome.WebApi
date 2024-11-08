using Microsoft.AspNetCore.Builder;
using SmartHome.Webservice.Extensions;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
var environment = builder.Environment;

builder
    .Services
    .AddSmarthomeServices(config);

var app = builder
    .Build()
    .ConfigureSmarthomeApp(environment);

app.Run();
