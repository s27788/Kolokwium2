using Kolokwium2;
using Kolokwium2.Data;
using Kolokwium2.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add DI for services
builder.Services.AddScoped<IRecordService, RecordService>();

var app = builder.Build();

// Inicjalizacja danych testowych (Student + Language)
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.Migrate(); // jeśli używasz migracji
    DbInitializer.Initialize(context); // dodaje rekordy tylko jeśli ich nie ma
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();