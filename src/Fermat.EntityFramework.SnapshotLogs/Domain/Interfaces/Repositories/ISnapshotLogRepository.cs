using Fermat.EntityFramework.Shared.Interfaces;
using Fermat.EntityFramework.SnapshotLogs.Domain.Entities;

namespace Fermat.EntityFramework.SnapshotLogs.Domain.Interfaces.Repositories;

public interface ISnapshotLogRepository : IRepository<SnapshotLog, Guid>
{
    Task<SnapshotLog?> GetLatestSnapshotLogAsync();
}