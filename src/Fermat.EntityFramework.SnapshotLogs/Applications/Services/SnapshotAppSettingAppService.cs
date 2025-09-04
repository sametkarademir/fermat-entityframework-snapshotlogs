using AutoMapper;
using Fermat.EntityFramework.Shared.DTOs.Pagination;
using Fermat.EntityFramework.Shared.Extensions;
using Fermat.EntityFramework.SnapshotLogs.Applications.DTOs.SnapshotAppSettings;
using Fermat.EntityFramework.SnapshotLogs.Domain.Interfaces.Repositories;
using Fermat.EntityFramework.SnapshotLogs.Domain.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Fermat.EntityFramework.SnapshotLogs.Applications.Services;

public class SnapshotAppSettingAppService(
    ISnapshotAppSettingRepository snapshotAppSettingRepository,
    IMapper mapper,
    ILogger<SnapshotAppSettingAppService> logger)
    : ISnapshotAppSettingAppService
{
    public async Task<SnapshotAppSettingResponseDto> GetByIdAsync(Guid snapshotLogId, Guid id)
    {
        var matchedSnapshotAppSetting = await snapshotAppSettingRepository.GetAsync(
            predicate: item => item.Id == id && item.SnapshotLogId == snapshotLogId,
            enableTracking: false
        );

        return mapper.Map<SnapshotAppSettingResponseDto>(matchedSnapshotAppSetting);
    }

    public async Task<PageableResponseDto<SnapshotAppSettingResponseDto>> GetPageableAndFilterAsync(Guid snapshotLogId, GetListSnapshotAppSettingRequestDto request)
    {
        var queryable = snapshotAppSettingRepository.GetQueryable();
        queryable = queryable.Where(item => item.SnapshotLogId == snapshotLogId);

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            queryable = queryable
                .Where(item =>
                    item.Key.Contains(request.Search) ||
                    item.Value.Contains(request.Search)
                );
        }

        queryable = queryable.AsNoTracking();
        queryable = queryable.ApplySort(request.Field, request.Order);
        var result = await queryable.ToPageableAsync(request.Page, request.PerPage);
        var matchedSnapshotAppSettings = mapper.Map<List<SnapshotAppSettingResponseDto>>(result.Data);

        return new PageableResponseDto<SnapshotAppSettingResponseDto>(matchedSnapshotAppSettings, result.Meta);
    }

    public async Task<int> CleanupOldSnapshotAppSettingAsync(Guid snapshotLogId, DateTime olderThan)
    {
        var queryable = snapshotAppSettingRepository.GetQueryable();
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
                    "[CleanupOldSnapshotAppSettingsAsync] [Action=DeleteRangeAsync()] [Count={Count}] [Start]",
                    countToDelete - totalDeleted
                );
                var snapshotAppSettingsToDelete = await queryable
                    .OrderBy(a => a.CreationTime)
                    .Take(batchSize)
                    .ToListAsync();

                if (snapshotAppSettingsToDelete.Count == 0)
                {
                    logger.LogInformation(
                        "[CleanupOldSnapshotAppSettingsAsync] [Action=DeleteRangeAsync()] [Count={Count}] [NoMoreLogsToDelete]",
                        totalDeleted
                    );

                    break;
                }

                await snapshotAppSettingRepository.DeleteRangeAsync(snapshotAppSettingsToDelete);
                await snapshotAppSettingRepository.SaveChangesAsync();

                logger.LogInformation(
                    "[CleanupOldSnapshotAppSettingsAsync] [Action=DeleteRangeAsync()] [Count={Count}] [End]",
                    snapshotAppSettingsToDelete.Count
                );

                totalDeleted += snapshotAppSettingsToDelete.Count;
                if (totalDeleted > 0 && totalDeleted % (batchSize * 5) == 0)
                {
                    await Task.Delay(500);
                }
            }
            catch (Exception e)
            {
                logger.LogError(
                    e,
                    "[CleanupOldSnapshotAppSettingsAsync] [Action=DeleteRangeAsync()] [Error] [Exception={Exception}]",
                    e.Message
                );

                break;
            }
        }

        return totalDeleted;
    }
}