using Fermat.EntityFramework.Shared.DTOs.Pagination;
using Fermat.EntityFramework.SnapshotLogs.Applications.DTOs.SnapshotAppSettings;
using Fermat.EntityFramework.SnapshotLogs.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fermat.EntityFramework.SnapshotLogs.Presentation.Controllers;

[ApiController]
[Route("api/snapshot-logs/{snapshotLogId:guid}/snapshot-app-settings")]
public class SnapshotAppSettingController(ISnapshotAppSettingAppService snapshotAppSettingAppService) : ControllerBase
{
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(SnapshotAppSettingResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<SnapshotAppSettingResponseDto>> GetByIdAsync([FromRoute(Name = "snapshotLogId")] Guid snapshotLogId, [FromRoute(Name = "id")] Guid id)
    {
        var result = await snapshotAppSettingAppService.GetByIdAsync(snapshotLogId, id);
        return Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(typeof(PageableResponseDto<SnapshotAppSettingResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> GetPageableAndFilterAsync([FromRoute(Name = "snapshotLogId")] Guid snapshotLogId, [FromQuery] GetListSnapshotAppSettingRequestDto request)
    {
        var result = await snapshotAppSettingAppService.GetPageableAndFilterAsync(snapshotLogId, request);
        return Ok(result);
    }

    [HttpDelete("cleanup")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CleanupOldSnapshotAppSettingsAsync([FromRoute(Name = "snapshotLogId")] Guid snapshotLogId, [FromQuery] DateTime olderThan)
    {
        var result = await snapshotAppSettingAppService.CleanupOldSnapshotAppSettingAsync(snapshotLogId, olderThan);
        return Ok(result);
    }
}