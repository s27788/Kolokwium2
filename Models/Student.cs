using System.ComponentModel.DataAnnotations;
namespace Kolokwium2.Models;

public class Student
{
    [Key]
    public int Id { get; set; }
    
    [MaxLength(100)]
    
    public string FirstName { get; set; } = null!;
    [MaxLength(100)]
    public string LastName { get; set; } = null!;
    [MaxLength(250)]

    public string Email { get; set; } = null!;
    
    public ICollection<Record> Records { get; set; } = new List<Record>();
}