using Fermat.EntityFramework.Shared.DTOs.Pagination;
using Fermat.EntityFramework.SnapshotLogs.Applications.DTOs.SnapshotAppSettings;

namespace Fermat.EntityFramework.SnapshotLogs.Domain.Interfaces.Services;

public interface ISnapshotAppSettingAppService
{
    Task<SnapshotAppSettingResponseDto> GetByIdAsync(Guid snapshotLogId, Guid id);
    Task<PageableResponseDto<SnapshotAppSettingResponseDto>> GetPageableAndFilterAsync(Guid snapshotLogId, GetListSnapshotAppSettingRequestDto request);
    Task<int> CleanupOldSnapshotAppSettingAsync(Guid snapshotLogId, DateTime olderThan);
}