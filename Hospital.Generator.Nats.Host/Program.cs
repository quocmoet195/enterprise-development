using Hospital.Generator.Nats.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = Host.CreateApplicationBuilder(args);


builder.Configuration
    .AddJsonFile("appsettings.json", optional: true)
    .AddEnvironmentVariables();

builder.Services.Configure<NatsOptions>(
    builder.Configuration.GetSection("Nats"));

builder.Services.AddHostedService<HospitalNatsProducer>();
builder.Services.AddLogging(c => c.AddConsole());

var host = builder.Build();
await host.RunAsync();
