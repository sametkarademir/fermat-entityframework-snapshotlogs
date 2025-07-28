using AutoMapper;
using Fermat.EntityFramework.SnapshotLogs.Applications.DTOs.SnapshotAppSettings;
using Fermat.EntityFramework.SnapshotLogs.Applications.DTOs.SnapshotAssemblies;
using Fermat.EntityFramework.SnapshotLogs.Applications.DTOs.SnapshotLogs;
using Fermat.EntityFramework.SnapshotLogs.Domain.Entities;

namespace Fermat.EntityFramework.SnapshotLogs.Applications.Profiles;

public class EntityProfile : Profile
{
    public EntityProfile()
    {
        CreateMap<SnapshotAppSetting, SnapshotAppSettingResponseDto>();
        CreateMap<SnapshotAssembly, SnapshotAssemblyResponseDto>();
        CreateMap<SnapshotLog, SnapshotLogResponseDto>();
    }
}