namespace Kolokwium2.Services;

using Microsoft.EntityFrameworkCore;
using Kolokwium2.Data;
using Kolokwium2.DTOs;
using Kolokwium2.Models;

public class RecordService : IRecordService
{
    private readonly AppDbContext _context;

    public RecordService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<RecordResponseDto>> GetRecordsAsync(int? languageId, int? taskId, DateTime? createdAfter)
    {
        var query = _context.Records
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
                ExecutionTime = r.ExecutionTime,
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
        // check student
        var student = await _context.Students.FindAsync(dto.StudentId);
        if (student is null)
            throw new ArgumentException("Student not found");

        // check language
        var language = await _context.Languages.FindAsync(dto.LanguageId);
        if (language is null)
            throw new ArgumentException("Language not found");

        // resolve task
        TaskModel? task = null;
        if (dto.TaskModel.Id is not null)
        {
            task = await _context.Tasks.FindAsync(dto.TaskModel.Id.Value);
            if (task == null && string.IsNullOrWhiteSpace(dto.TaskModel.Name))
                throw new ArgumentException("Task not found and name not provided");
        }

        if (task == null && !string.IsNullOrWhiteSpace(dto.TaskModel.Name) && !string.IsNullOrWhiteSpace(dto.TaskModel.Description))
        {
            task = new TaskModel
            {
                Name = dto.TaskModel.Name!,
                Description = dto.TaskModel.Description!
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
        }

        if (task == null)
            throw new ArgumentException("Task resolution failed");

        var record = new Record
        {
            CreatedAt = dto.Created,
            ExecutionTime = dto.ExecutionTime,
            StudentId = dto.StudentId,
            LanguageId = dto.LanguageId,
            TaskId = task.Id
        };

        _context.Records.Add(record);
        await _context.SaveChangesAsync();

        return record.Id;
    }
}
