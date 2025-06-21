using System.ComponentModel.DataAnnotations;

namespace Kolokwium2.Models;

public class TaskModel
{
    [Key]
    public int Id { get; set; }
    
    [MaxLength(100)]
    public string Name { get; set; } = null!;
    
    [MaxLength(2000)]
    public string Description { get; set; } = null!;
    
    public ICollection<Record> Records { get; set; } = new List<Record>();
}