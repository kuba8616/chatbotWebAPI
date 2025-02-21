using ChatbotAI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatbotAI.Infrastructure.Persistance.Configurations;
public class ChatResponseConfiguration : IEntityTypeConfiguration<ChatResponse>
{
    public void Configure(EntityTypeBuilder<ChatResponse> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id)
               .UseIdentityColumn();

        builder.HasOne(r => r.ChatMessage)
               .WithMany()
               .HasForeignKey(r => r.ChatMessageId);
    }
}
