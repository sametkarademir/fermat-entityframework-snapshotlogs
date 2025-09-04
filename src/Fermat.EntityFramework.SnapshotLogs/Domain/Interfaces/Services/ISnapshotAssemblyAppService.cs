using Fermat.EntityFramework.Shared.DTOs.Pagination;
using Fermat.EntityFramework.SnapshotLogs.Applications.DTOs.SnapshotAssemblies;

namespace Fermat.EntityFramework.SnapshotLogs.Domain.Interfaces.Services;

public interface ISnapshotAssemblyAppService
{
    Task<SnapshotAssemblyResponseDto> GetByIdAsync(Guid snapshotLogId, Guid id);
    Task<PageableResponseDto<SnapshotAssemblyResponseDto>> GetPageableAndFilterAsync(Guid snapshotLogId, GetListSnapshotAssemblyRequestDto request);
    Task<int> CleanupOldSnapshotAssemblyAsync(Guid snapshotLogId, DateTime olderThan);
}