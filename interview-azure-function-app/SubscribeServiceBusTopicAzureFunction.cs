using Azure.Messaging.ServiceBus;
using interview_azure_function_app.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;


namespace interview_azure_function_app;


/// <summary>
/// This azure function is subscribing to azure service bus topic named 'oddeven'
/// It gets triggered when a message published to azure service bus
/// </summary>
public class SubscribeServiceBusTopicAzureFunction
{
    private readonly ILogger<SubscribeServiceBusTopicAzureFunction> _logger;
    private static readonly HttpClient _httpClient = new HttpClient
    {
        Timeout = TimeSpan.FromSeconds(20)
    };

    public SubscribeServiceBusTopicAzureFunction(ILogger<SubscribeServiceBusTopicAzureFunction> logger)
    {
        _logger = logger;
    }

    [Function(nameof(SubscribeServiceBusTopicAzureFunction))]
    public async Task Run(
        [ServiceBusTrigger("oddeven", "oddEvenSubsriber", Connection = "ServiceBusConnection")]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions)
    {
        _logger.LogInformation("Message ID: {id}", message.MessageId);
        _logger.LogInformation("Message Body: {body}", message.Body);
        _logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);

        try
        {
            // Convert BinaryData to string
            var jsonBody = message.Body.ToString();

            _logger.LogInformation("Json on message {MessageId} body: {body}", message.MessageId, jsonBody);

            // Deserialize to a dynamic object
            var azServiceBusMessagePayload = JsonSerializer.Deserialize<AzServiceBusMessagePayload>(jsonBody,
                                                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                                                );
            if (azServiceBusMessagePayload == null)
            {
                _logger.LogError("Error in Deserialization: Json Body from Service Bus {jsonBody} for message ID: {MessageId}", jsonBody, message.MessageId);
                throw new FormatException($"Error in Deserialization: Json Body from Service Bus: {jsonBody} for message ID: {message.MessageId}");
            }

            _logger.LogInformation("Deserialization Complete: Number is {Number}", azServiceBusMessagePayload.Number);

            if (azServiceBusMessagePayload.Number % 2 == 0)
            {
                _logger.LogInformation("---> Service Bus MessageId {MessageId} Number {Number} is EVEN", message.MessageId, azServiceBusMessagePayload.Number);
                await PostToEndPoint(azServiceBusMessagePayload.Number, "https://number-api-gcgcagdadwb8e9f8.australiaeast-01.azurewebsites.net/api/numbers");
            }
            else
            {
                _logger.LogInformation("---> Service Bus MessageId {MessageId} Number {Number} is ODD", message.MessageId, azServiceBusMessagePayload.Number);
                await PostToEndPoint(azServiceBusMessagePayload.Number, "http://docker-number-minimal-api.heedhde8cgb2apgs.australiaeast.azurecontainer.io/api/docker-numbers");
            }

            // Complete the message
            await messageActions.CompleteMessageAsync(message);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Could not post number items to API end points");
        }
    }

    private async Task PostToEndPoint(int num, string url)
    {
        var numItem = new ApiPostNumItem { Number = num };
        var json = JsonSerializer.Serialize(numItem);
        _logger.LogInformation("Json to post: {json} to URL {url}", json, url);

        var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(url, stringContent);
        var responseMessage = await response.Content.ReadAsStringAsync();
        _logger.LogInformation("Response Message from URL {url} is {responseMessage}", url, responseMessage);
    }
}