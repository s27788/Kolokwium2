using Kolokwium2.DTOs;

public class RecordResponseDto
{
    public int Id { get; set; }
    public string ExecutionTime { get; set; } = null!; // ✅ WAŻNE!
    public DateTime Created { get; set; }
    public LanguageDto Language { get; set; } = null!;
    public StudentDto Student { get; set; } = null!;
    public TaskModelDto TaskModel { get; set; } = null!;
}