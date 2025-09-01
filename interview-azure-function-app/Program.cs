using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();


builder.Services.AddLogging(logging =>
{
    logging.AddConsole();
});

builder.Services.AddSingleton<ServiceBusClient>(provider =>
{
    var connectionString = Environment.GetEnvironmentVariable("ServiceBusConnection");      // this is coming from Azure Function App Enviroment Variables
    return new ServiceBusClient(connectionString);
});

builder.Services.AddSingleton<ServiceBusSender>(provider =>
{
    var client = provider.GetRequiredService<ServiceBusClient>();
    return client.CreateSender("oddeven");
});


builder.Build().Run();
