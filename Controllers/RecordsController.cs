using Microsoft.AspNetCore.Mvc;
using Kolokwium2.DTOs;
using Kolokwium2.Services;

namespace Kolokwium2.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RecordsController(IRecordService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetRecords([FromQuery] int? languageId, [FromQuery] int? taskId, [FromQuery] DateTime? createdAfter)
    {
        var records = await service.GetRecordsAsync(languageId, taskId, createdAfter);
        return Ok(records);
    }

    [HttpPost]
    public async Task<IActionResult> AddRecord([FromBody] RecordRequestDto dto)
    {
        try
        {
            var id = await service.AddRecordAsync(dto);
            return CreatedAtAction(nameof(GetRecords), new { id = id }, new { id = id });
        }
        catch (ArgumentException e)
        {
            return BadRequest(new { error = e.Message });
        }
    }
}