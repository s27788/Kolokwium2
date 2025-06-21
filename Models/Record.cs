using System.ComponentModel.DataAnnotations;

namespace Kolokwium2.Models;
public class Record
{
    [Key]
    public int Id { get; set; }
    public int LanguageId { get; set; }
    public int StudentId { get; set; }
    public int TaskId { get; set; }
    public long ExecutionTime { get; set; }
    public DateTime CreatedAt { get; set; }

    public Language Language { get; set; } = null!;
    public Student Student { get; set; } = null!;
    public TaskModel TaskModel { get; set; } = null!;
}