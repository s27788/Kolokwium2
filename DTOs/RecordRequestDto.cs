using System.Text.Json.Serialization;

public class RecordRequestDto
{
    public DateTime Created { get; set; }
    public long ExecutionTime { get; set; }

    public int StudentId { get; set; }
    public int LanguageId { get; set; }

    [JsonPropertyName("task")]
    public TaskModelDto TaskModel { get; set; }
}
