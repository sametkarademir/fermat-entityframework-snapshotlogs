using AutoMapper;
using Fermat.EntityFramework.Shared.DTOs.Pagination;
using Fermat.EntityFramework.Shared.Extensions;
using Fermat.EntityFramework.SnapshotLogs.Applications.DTOs.SnapshotAssemblies;
using Fermat.EntityFramework.SnapshotLogs.Domain.Interfaces.Repositories;
using Fermat.EntityFramework.SnapshotLogs.Domain.Interfaces.Services;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Fermat.EntityFramework.SnapshotLogs.Applications.Services;

public class SnapshotAssemblyAppService(
    ISnapshotAssemblyRepository snapshotAssemblyRepository,
    IMapper mapper,
    ILogger<SnapshotAssemblyAppService> logger)
    : ISnapshotAssemblyAppService
{
    public async Task<SnapshotAssemblyResponseDto> GetByIdAsync(Guid snapshotLogId, Guid id)
    {
        var matchedSnapshotAssembly = await snapshotAssemblyRepository.GetAsync(
            predicate: item => item.Id == id && item.SnapshotLogId == snapshotLogId,
            enableTracking: false
        );

        return mapper.Map<SnapshotAssemblyResponseDto>(matchedSnapshotAssembly);
    }

    public async Task<PageableResponseDto<SnapshotAssemblyResponseDto>> GetPageableAndFilterAsync(Guid snapshotLogId, GetListSnapshotAssemblyRequestDto request)
    {
        var queryable = snapshotAssemblyRepository.GetQueryable();
        queryable = queryable.Where(item => item.SnapshotLogId == snapshotLogId);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            queryable = queryable
                .Where(item =>
                    item.Name != null && item.Name.Contains(request.Search)
                );
        }

        queryable = queryable.AsNoTracking();
        queryable = queryable.ApplySort(request.Field, request.Order);
        var result = await queryable.ToPageableAsync(request.Page, request.PerPage);
        var matchedSnapshotAssemblies = mapper.Map<List<SnapshotAssemblyResponseDto>>(result.Data);

        return new PageableResponseDto<SnapshotAssemblyResponseDto>(matchedSnapshotAssemblies, result.Meta);
    }

    public async Task<int> CleanupOldSnapshotAssemblyAsync(Guid snapshotLogId, DateTime olderThan)
    {
        var queryable = snapshotAssemblyRepository.GetQueryable();
        queryable = queryable.Where(a => a.CreationTime < olderThan && a.SnapshotLogId == snapshotLogId);
        var countToDelete = await queryable.CountAsync();
        if (countToDelete == 0)
        {
            return 0;
        }

        const int batchSize = 100;
        var totalDeleted = 0;
        while (countToDelete > totalDeleted)
        {
            try
            {
                logger.LogInformation(
                    "[CleanupOldSnapshotAssemblyAsync] [Action=DeleteRangeAsync()] [Count={Count}] [Start]",
                    countToDelete - totalDeleted
                );
                var snapshotAssembliesToDelete = await queryable
                    .OrderBy(a => a.CreationTime)
                    .Take(batchSize)
                    .ToListAsync();

                if (snapshotAssembliesToDelete.Count == 0)
                {
                    logger.LogInformation(
                        "[CleanupOldSnapshotAssemblyAsync] [Action=DeleteRangeAsync()] [Count={Count}] [NoMoreLogsToDelete]",
                        totalDeleted
                    );

                    break;
                }

                await snapshotAssemblyRepository.DeleteRangeAsync(snapshotAssembliesToDelete);
                await snapshotAssemblyRepository.SaveChangesAsync();

                logger.LogInformation(
                    "[CleanupOldSnapshotAssemblyAsync] [Action=DeleteRangeAsync()] [Count={Count}] [End]",
                    snapshotAssembliesToDelete.Count
                );

                totalDeleted += snapshotAssembliesToDelete.Count;
                if (totalDeleted > 0 && totalDeleted % (batchSize * 5) == 0)
                {
                    await Task.Delay(500);
                }
            }
            catch (Exception e)
            {
                logger.LogError(
                    e,
                    "[CleanupOldSnapshotAssemblyAsync] [Action=DeleteRangeAsync()] [Error] [Exception={Exception}]",
                    e.Message
                );

                break;
            }
        }

        return totalDeleted;
    }
}