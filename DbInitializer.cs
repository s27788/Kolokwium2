using Kolokwium2.Data;
using Kolokwium2.Models;

namespace Kolokwium2;

public static class DbInitializer
{
    public static void Initialize(AppDbContext context)
    {
        context.Database.EnsureCreated(); 

        if (context.Students.Any()) return; 

        var student = new Student
        {
            FirstName = "Adam",
            LastName = "Miller",
            Email = "adam.miller@mail.com"
        };

        var language = new Language
        {
            Name = "C#"
        };

        context.Students.Add(student);
        context.Languages.Add(language);
        context.SaveChanges();
    }
}