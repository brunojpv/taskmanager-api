using System.Text.Json;
using System.Text.Json.Serialization;
using DomainTaskStatus = TaskManager.Domain.Entities.TaskStatus;

namespace TaskManager.Domain.Converters
{
    public class TaskStatusConverter : JsonConverter<DomainTaskStatus>
    {
        public override DomainTaskStatus Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString()?.ToLowerInvariant();
            return value switch
            {
                "pending" => DomainTaskStatus.Pending,
                "inprogress" => DomainTaskStatus.InProgress,
                "completed" => DomainTaskStatus.Completed,
                _ => throw new JsonException("Invalid task status.")
            };
        }

        public override void Write(Utf8JsonWriter writer, DomainTaskStatus value, JsonSerializerOptions options)
        {
            var statusString = value switch
            {
                DomainTaskStatus.Pending => "pending",
                DomainTaskStatus.InProgress => "inProgress",
                DomainTaskStatus.Completed => "completed",
                _ => throw new JsonException("Invalid task status.")
            };

            writer.WriteStringValue(statusString);
        }
    }
}
