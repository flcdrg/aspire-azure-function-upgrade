using MongoDB.Bson;

namespace aspire_ts_cs_starter.Function;

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