using Fermat.EntityFramework.Shared.DTOs.Pagination;
using Fermat.EntityFramework.SnapshotLogs.Applications.DTOs.SnapshotAppSettings;

namespace Fermat.EntityFramework.SnapshotLogs.Domain.Interfaces.Services;

public interface ISnapshotAppSettingAppService
{
    Task<SnapshotAppSettingResponseDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PageableResponseDto<SnapshotAppSettingResponseDto>> GetPageableAndFilterAsync(GetListSnapshotAppSettingRequestDto request, CancellationToken cancellationToken = default);
    Task<int> CleanupOldSnapshotAppSettingAsync(DateTime olderThan, CancellationToken cancellationToken = default);
}