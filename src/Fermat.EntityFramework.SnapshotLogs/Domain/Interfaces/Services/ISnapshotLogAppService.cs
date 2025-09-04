using Fermat.EntityFramework.Shared.DTOs.Pagination;
using Fermat.EntityFramework.SnapshotLogs.Applications.DTOs.SnapshotLogs;

namespace Fermat.EntityFramework.SnapshotLogs.Domain.Interfaces.Services;

public interface ISnapshotLogAppService
{
    Task<SnapshotLogResponseDto> GetByIdAsync(Guid id);
    Task<PageableResponseDto<SnapshotLogResponseDto>> GetPageableAndFilterAsync(GetListSnapshotLogRequestDto request);
    Task<int> CleanupOldSnapshotLogAsync(DateTime olderThan);
}