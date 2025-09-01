using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace interview_azure_function_app;

/// <summary>
/// Publisher to Azure Service Bus
/// </summary>
public class PushToAzureServiceBus
{
    private readonly ILogger<PushToAzureServiceBus> _logger;
    private readonly ServiceBusSender _sender;

    public PushToAzureServiceBus(ILogger<PushToAzureServiceBus> logger, ServiceBusSender sender)
    {
        _logger = logger;
        _sender = sender;
    }

    [Function("PushToAzureServiceBus")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
    {
        _logger.LogInformation("Processing request...");

        var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        using var messageBatch = await _sender.CreateMessageBatchAsync();

        if (!messageBatch.TryAddMessage(new ServiceBusMessage(requestBody)))
        {
            _logger.LogError("Message too large for batch.");
            return new BadRequestObjectResult("Message too large.");
        }

        try
        {
            await _sender.SendMessagesAsync(messageBatch);
            _logger.LogInformation("Batch sent to topic.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send batch.");
            return new StatusCodeResult(500);
        }

        return new OkObjectResult("Payload received");
    }

}