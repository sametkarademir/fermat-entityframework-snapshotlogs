using Fermat.EntityFramework.Shared.Interfaces;
using Fermat.EntityFramework.SnapshotLogs.Domain.Entities;

namespace Fermat.EntityFramework.SnapshotLogs.Domain.Interfaces.Repositories;

public interface ISnapshotAppSettingRepository : IRepository<SnapshotAppSetting, Guid>
{
}