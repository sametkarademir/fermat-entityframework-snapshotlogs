using Fermat.Domain.Shared.Auditing;
using Fermat.Domain.Shared.Filters;

namespace Fermat.EntityFramework.SnapshotLogs.Domain.Entities;

[ExcludeFromProcessing]
public class SnapshotAppSetting : CreationAuditedEntity<Guid>
{
    public string Key { get; set; } = null!;
    public string Value { get; set; } = null!;

    public Guid SnapshotLogId { get; set; }
    public SnapshotLog? SnapshotLog { get; set; }

    public SnapshotAppSetting() : base(Guid.NewGuid())
    {
        CreationTime = DateTime.UtcNow;
    }
}