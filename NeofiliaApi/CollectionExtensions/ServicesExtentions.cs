using NeofiliaApi.Services;
using NeofiliaDomain.Application.Common.Services;

namespace NeofiliaApi.CollectionExtensions;

public static class ServicesExtentions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IRewardService, RewardService>();
        services.AddSingleton<IRewardService, RewardService>();
        return services;
    }
}
