using ChatbotAI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChatbotAI.Application.Common;
public interface IApplicationDbContext
{
    DbSet<ChatMessage> ChatMessages { get; }

    DbSet<ChatResponse> ChatResponses { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
