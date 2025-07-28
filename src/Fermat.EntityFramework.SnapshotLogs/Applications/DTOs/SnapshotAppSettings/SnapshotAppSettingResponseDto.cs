using Fermat.Domain.Shared.Auditing;

namespace Fermat.EntityFramework.SnapshotLogs.Applications.DTOs.SnapshotAppSettings;

public class SnapshotAppSettingResponseDto : CreationAuditedEntity<Guid>
{
    public string Key { get; set; } = null!;
    public string Value { get; set; } = null!;

    public Guid SnapshotLogId { get; set; }
}