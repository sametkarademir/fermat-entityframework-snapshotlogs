using Fermat.EntityFramework.Shared.Repositories;
using Fermat.EntityFramework.SnapshotLogs.Domain.Entities;
using Fermat.EntityFramework.SnapshotLogs.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Fermat.EntityFramework.SnapshotLogs.Infrastructure.Repositories;

public class SnapshotAssemblyRepository<TContext> : EfRepositoryBase<SnapshotAssembly, Guid, TContext>, ISnapshotAssemblyRepository where TContext : DbContext
{
    public SnapshotAssemblyRepository(TContext dbContext) : base(dbContext)
    {
    }
}