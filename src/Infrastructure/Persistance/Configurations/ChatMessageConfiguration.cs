using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatbotAI.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

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

       
    }
}
