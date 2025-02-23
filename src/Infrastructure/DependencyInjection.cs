using ChatbotAI.Application.Common;
using ChatbotAI.Application.Interfaces.Services;
using ChatbotAI.Infrastructure.Persistance;
using ChatbotAI.Infrastructure.Persistance.Interceptors;
using ChatbotAI.Infrastructure.Services.Chatbot;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<ApplicationDbContext>(options =>
           options.UseSqlServer(connectionString));

        services.AddScoped<ISaveChangesInterceptor, CreatedAtEntityInterceptor>();
        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        services.AddScoped<ApplicationDbContextInitialiser>();
        services.AddSingleton(TimeProvider.System);
        
        services.AddServices();

        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton<IChatbotResponseGenerator, SimpleChatbotResponseGenerator>();

        return services;
    }
}
