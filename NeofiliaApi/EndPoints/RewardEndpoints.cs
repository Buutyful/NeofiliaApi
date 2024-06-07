using Microsoft.AspNetCore.Http.HttpResults;
using NeofiliaDomain;
using NeofiliaDomain.Application.Common.Repositories;
using NeofiliaDomain.Application.Common.Services;

namespace NeofiliaApi.EndPoints;

public static class RewardEndpoints
{
    public static void MapRewardEndPoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/rewards").WithOpenApi();

        group.MapGet("", async (IRewardRepository repo) =>
        {
            var rewards = await repo.Get();
            return Results.Ok(rewards);
        });

        group.MapGet("{id:guid}",
            async Task<Results<
                Ok<RewardDto>,
                    NotFound>> (Guid id, IRewardRepository repo) =>
            {
                var reward = await repo.GetById(id);
                return reward is null ?
                TypedResults.NotFound() :
                TypedResults.Ok(reward.ToDto());
            });
        group.MapGet("{tableId:int}",
           async (int tableId, IRewardRepository repo) =>
           {
               var rewards = await repo.GetByTableId(tableId);
               return Results.Ok(rewards.Select(r => r.ToDto()));
           });

        group.MapPost("{id:guid}", 
            async Task<Results<
                Ok,
                BadRequest>> (Guid id, IRewardService service) =>
            {
                try
                {
                    await service.RedeemReward(id);
                    return TypedResults.Ok();
                }
                catch (ArgumentException ex)
                {
                    return TypedResults.BadRequest();
                }
            });
    }
}

public record RewardDto(Guid Id, bool IsRedeemed);

public static class MapReward
{
    public static RewardDto ToDto(this TableReward reward) =>
        new(reward.Id, reward.IsRedeemed);
}