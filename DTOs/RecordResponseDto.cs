// DTOs/RecordResponseDto.cs

using System.Text.Json.Serialization;

namespace Kolokwium2.DTOs;

public class RecordResponseDto
{
    public int Id { get; set; }
    public long ExecutionTime { get; set; }
    public DateTime Created { get; set; }
    public LanguageDto Language { get; set; } = null!;
    public StudentDto Student { get; set; } = null!;
    
    [JsonPropertyName("task")]
    public TaskModelDto TaskModel { get; set; }
}