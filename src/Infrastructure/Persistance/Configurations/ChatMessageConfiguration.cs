using ChatbotAI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatbotAI.Infrastructure.Persistance.Configurations;
public class ChatMessageConfiguration : IEntityTypeConfiguration<ChatMessage>
{
    public void Configure(EntityTypeBuilder<ChatMessage> builder)
    {
        builder.HasKey(m => m.Id);

        builder.Property(m => m.Id)
               .UseIdentityColumn();

        builder.Property(m => m.Content)
               .IsRequired()
               .HasMaxLength(4000);

        builder.Property(m => m.CreatedAt)
               .IsRequired();

        builder.HasOne(m => m.Response)
           .WithOne(r => r.ChatMessage)
           .HasForeignKey<ChatResponse>(r => r.ChatMessageId);


    }
}
