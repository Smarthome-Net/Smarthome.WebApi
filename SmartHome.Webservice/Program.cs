using Microsoft.AspNetCore.Builder;
using SmartHome.Webservice.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder
    .Services
    .AddSmartHomeServices(builder.Configuration);

var app = builder
    .Build()
    .ConfigureSmartHomeApp(builder.Environment);

app.Run();
