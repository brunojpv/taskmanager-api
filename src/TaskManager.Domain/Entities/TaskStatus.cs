using System.Text.Json.Serialization;
using TaskManager.Domain.Converters;

namespace TaskManager.Domain.Entities
{
    [JsonConverter(typeof(TaskStatusConverter))]
    public enum TaskStatus
    {
        Pending = 0,
        InProgress = 1,
        Completed = 2
    }
}
