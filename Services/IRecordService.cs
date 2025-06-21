using Kolokwium2.DTOs;

namespace Kolokwium2.Services;

public interface IRecordService
{
    Task<IEnumerable<RecordResponseDto>> GetRecordsAsync(int? languageId, int? taskId, DateTime? createdAfter);
    Task<int> AddRecordAsync(RecordRequestDto dto);
}