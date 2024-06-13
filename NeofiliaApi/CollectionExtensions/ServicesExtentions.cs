using NeofiliaApi.Hubs;
using NeofiliaDomain.Application.Common.Hubs;
using NeofiliaDomain.Application.Services.Reward;

namespace NeofiliaApi.CollectionExtensions;

public static class ServicesExtentions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {        
        services.AddSingleton<IRewardService, RewardService>();
        services.AddScoped<IRewardHubService, RewardHubService>();
        return services;
    }
}
