using Kolokwium2.Data;
using Kolokwium2.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IRecordService, RecordService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();

    if (!db.Students.Any())
    {
        db.Students.Add(new Kolokwium2.Models.Student
        {
            FirstName = "Adam",
            LastName = "Miller",
            Email = "adam.miller@mail.com"
        });
    }

    if (!db.Languages.Any())
    {
        db.Languages.Add(new Kolokwium2.Models.Language
        {
            Name = "C#"
        });
    }

    db.SaveChanges();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseAuthorization();
app.MapControllers();
app.Run();