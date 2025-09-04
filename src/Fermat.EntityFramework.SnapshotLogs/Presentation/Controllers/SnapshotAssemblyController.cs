using Fermat.EntityFramework.Shared.DTOs.Pagination;
using Fermat.EntityFramework.SnapshotLogs.Applications.DTOs.SnapshotAssemblies;
using Fermat.EntityFramework.SnapshotLogs.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fermat.EntityFramework.SnapshotLogs.Presentation.Controllers;

[ApiController]
[Route("api/snapshot-logs/{snapshotLogId:guid}/snapshot-assemblies")]
public class SnapshotAssemblyController(ISnapshotAssemblyAppService snapshotAssemblyAppService) : ControllerBase
{
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(SnapshotAssemblyResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<SnapshotAssemblyResponseDto>> GetByIdAsync([FromRoute(Name = "snapshotLogId")] Guid snapshotLogId, [FromRoute(Name = "id")] Guid id)
    {
        var result = await snapshotAssemblyAppService.GetByIdAsync(snapshotLogId, id);
        return Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(typeof(PageableResponseDto<SnapshotAssemblyResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> GetPageableAndFilterAsync([FromRoute(Name = "snapshotLogId")] Guid snapshotLogId, [FromQuery] GetListSnapshotAssemblyRequestDto request)
    {
        var result = await snapshotAssemblyAppService.GetPageableAndFilterAsync(snapshotLogId, request);
        return Ok(result);
    }

    [HttpDelete("cleanup")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CleanupOldSnapshotAssembliesAsync([FromRoute(Name = "snapshotLogId")] Guid snapshotLogId, [FromQuery] DateTime olderThan)
    {
        var result = await snapshotAssemblyAppService.CleanupOldSnapshotAssemblyAsync(snapshotLogId, olderThan);
        return Ok(result);
    }
}