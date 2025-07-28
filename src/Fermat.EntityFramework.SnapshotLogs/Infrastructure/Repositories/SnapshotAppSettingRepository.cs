using Fermat.EntityFramework.Shared.Repositories;
using Fermat.EntityFramework.SnapshotLogs.Domain.Entities;
using Fermat.EntityFramework.SnapshotLogs.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Fermat.EntityFramework.SnapshotLogs.Infrastructure.Repositories;

public class SnapshotAppSettingRepository<TContext> : EfRepositoryBase<SnapshotAppSetting, Guid, TContext>, ISnapshotAppSettingRepository where TContext : DbContext
{
    public SnapshotAppSettingRepository(TContext dbContext) : base(dbContext)
    {
    }
}