using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoDB.Bson;

namespace aspire_ts_cs_starter.Function;

public class HttpTrigger
{
    private readonly ILogger<HttpTrigger> _logger;
    private readonly IMongoClient _client;

    public HttpTrigger(ILogger<HttpTrigger> logger, IMongoClient client)
    {
        _logger = logger;
        _client = client;
    }

    [Function("HttpTrigger")]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");

        // Add data to MongoDb
        var database = _client.GetDatabase("auditing");
        var collection = database.GetCollection<AuditEntry>("logs");

        var document = GetDocument();

        collection.InsertOne(document);

        return new OkObjectResult("Welcome to Azure Functions!");
    }

    private static AuditEntry GetDocument()
    {
        return new AuditEntry
            {
            Message = "Function invoked"
        };
    }
}

public class AuditEntry
{
    private string _message;
    public ObjectId Id { get; set; }

    public required string Message
    {
        get => _message;
        set => _message = value ?? string.Empty;
    }

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}