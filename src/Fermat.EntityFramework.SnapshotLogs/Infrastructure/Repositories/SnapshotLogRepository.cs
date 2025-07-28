using Fermat.EntityFramework.Shared.Repositories;
using Fermat.EntityFramework.SnapshotLogs.Domain.Entities;
using Fermat.EntityFramework.SnapshotLogs.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Fermat.EntityFramework.SnapshotLogs.Infrastructure.Repositories;

public class SnapshotLogRepository<TContext> : EfRepositoryBase<SnapshotLog, Guid, TContext>, ISnapshotLogRepository where TContext : DbContext
{
    public SnapshotLogRepository(TContext dbContext) : base(dbContext)
    {
    }

    public async Task<SnapshotLog?> GetLatestSnapshotLogAsync()
    {
        var query = GetQueryable();
        var latestSnapshot = await query.OrderByDescending(s => s.CreationTime).FirstOrDefaultAsync();

        return latestSnapshot;
    }
}