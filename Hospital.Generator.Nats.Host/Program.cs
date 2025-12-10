using Hospital.Generator.Nats.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);
builder.AddServiceDefaults();
builder.AddNatsClient("hospital-nats");
builder.Services.Configure<NatsOptions>(builder.Configuration.GetSection("Nats"));
builder.Services.AddHostedService<HospitalNatsProducer>();
builder.Services.AddOptions<NatsOptions>()
    .Bind(builder.Configuration.GetSection("Nats"));
builder.Services.AddHostedService<HospitalNatsProducer>();

var host = builder.Build();
await host.RunAsync();
