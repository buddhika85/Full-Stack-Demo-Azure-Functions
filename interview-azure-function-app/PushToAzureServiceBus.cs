using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace interview_azure_function_app;

public class PushToAzureServiceBus
{
    private readonly ILogger<PushToAzureServiceBus> _logger;

    public PushToAzureServiceBus(ILogger<PushToAzureServiceBus> logger)
    {
        _logger = logger;
    }

    [Function("PushToAzureServiceBus")]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        return new OkObjectResult("Welcome to Azure Functions!");
    }
}