namespace Kolokwium2.Services;

using Microsoft.EntityFrameworkCore;
using Kolokwium2.Data;
using Kolokwium2.DTOs;
using Kolokwium2.Models;

public class RecordService(AppDbContext context) : IRecordService
{
    public async Task<IEnumerable<RecordResponseDto>> GetRecordsAsync(int? languageId, int? taskId, DateTime? createdAfter)
    {
        var query = context.Records
            .Include(r => r.Language)
            .Include(r => r.Student)
            .Include(r => r.TaskModel)
            .AsQueryable();

        if (languageId is not null)
            query = query.Where(r => r.LanguageId == languageId);

        if (taskId is not null)
            query = query.Where(r => r.TaskId == taskId);

        if (createdAfter is not null)
            query = query.Where(r => r.CreatedAt >= createdAfter);

        return await query
            .OrderByDescending(r => r.CreatedAt)
            .ThenBy(r => r.Student.LastName)
            .Select(r => new RecordResponseDto
            {
                Id = r.Id,
                ExecutionTime = r.ExecutionTime.ToString(),
                Created = r.CreatedAt,
                Language = new LanguageDto { Id = r.Language.Id, Name = r.Language.Name },
                Student = new StudentDto
                {
                    Id = r.Student.Id,
                    FirstName = r.Student.FirstName,
                    LastName = r.Student.LastName,
                    Email = r.Student.Email
                },
                TaskModel = new TaskModelDto
                {
                    Id = r.TaskModel.Id,
                    Name = r.TaskModel.Name,
                    Description = r.TaskModel.Description
                }
            })
            .ToListAsync();
    }

    public async Task<int> AddRecordAsync(RecordRequestDto dto)
    {
        var student = await context.Students.FindAsync(dto.StudentId);
        if (student is null)
            throw new ArgumentException("student not found");

        var language = await context.Languages.FindAsync(dto.LanguageId);
        if (language is null)
            throw new ArgumentException("language not found");

        TaskModel? task = null;
        if (dto.TaskModel.Id is not null)
        {
            task = await context.Tasks.FindAsync(dto.TaskModel.Id.Value);
            if (task == null && string.IsNullOrWhiteSpace(dto.TaskModel.Name))
                throw new ArgumentException("Task not found, name not provided");
        }

        if (task == null && !string.IsNullOrWhiteSpace(dto.TaskModel.Name) && !string.IsNullOrWhiteSpace(dto.TaskModel.Description))
        {
            task = new TaskModel
            {
                Name = dto.TaskModel.Name!,
                Description = dto.TaskModel.Description!
            };

            context.Tasks.Add(task);
            await context.SaveChangesAsync();
        }

        if (task == null)
            throw new ArgumentException("Task failed");

        var record = new Record
        {
            CreatedAt = dto.Created,
            ExecutionTime = dto.ExecutionTime,
            StudentId = dto.StudentId,
            LanguageId = dto.LanguageId,
            TaskId = task.Id
        };

        context.Records.Add(record);
        await context.SaveChangesAsync();

        return record.Id;
    }
}
