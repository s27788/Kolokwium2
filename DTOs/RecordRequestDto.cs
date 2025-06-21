public class RecordRequestDto


{
    public long ExecutionTime { get; set; }
    public DateTime Created { get; set; }
    public int StudentId { get; set; }
    public int LanguageId { get; set; }
    public TaskModelDto TaskModel { get; set; } = null!;
}