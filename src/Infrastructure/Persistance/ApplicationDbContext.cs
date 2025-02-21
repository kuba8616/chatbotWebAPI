using System.Reflection;
using ChatbotAI.Application.Common;
using ChatbotAI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChatbotAI.Infrastructure.Persistance;
public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<ChatMessage> ChatMessages { get; set; } = null!;
    public DbSet<ChatResponse> ChatResponses { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}

