using Microsoft.AspNetCore.Builder;

namespace Fermat.EntityFramework.SnapshotLogs.DependencyInjection;

public static class ApplicationBuilderExceptionMiddlewareExtensions
{
    public static void FermatSnapshotLogMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<ProgressingStartedMiddleware>();
    }
}
