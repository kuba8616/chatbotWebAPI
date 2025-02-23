using ChatbotAI.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ChatbotAI.Infrastructure.Persistance.Interceptors;
internal class CreatedAtEntityInterceptor : SaveChangesInterceptor
{
    private readonly TimeProvider _dateTime;

    public CreatedAtEntityInterceptor(TimeProvider dateTime)
    {
        _dateTime = dateTime;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public void UpdateEntities(DbContext? context)
    {
        if (context == null) return;

        foreach (var entry in context.ChangeTracker.Entries<ICreatedAt>())
        {
            if (entry.State is EntityState.Added)
            {
                var utcNow = _dateTime.GetUtcNow();
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = utcNow.Date;
                }
            }
        }
    }
}
