using FPTS.FIT.BDRD.BuildingBlocks.SharedKernel.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FPTS.FIT.BDRD.BuildingBlocks.SharedKernel.Extensions;

public static class MediatorExtension
{
    /// <summary>
    /// Khi dùng ef core có DbContext mới dùng dc
    /// </summary>
    /// <param name="mediator"></param>
    /// <param name="ctx"></param>
    /// <returns></returns>
    public static async Task DispatchDomainEventsAsync(this IMediator mediator, DbContext ctx)
    {
        var domainEntities = ctx.ChangeTracker
            .Entries<BaseEntity>()
            .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any());

        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.DomainEvents)
            .ToList();

        domainEntities.ToList()
            .ForEach(entity => entity.Entity.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
            await mediator.Publish(domainEvent);
    }

    /// <summary>
    /// Được tạo thêm để dùng khi không dùng DbContext
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="mediator"></param>
    /// <param name="t"></param>
    /// <returns></returns>
    public static async Task DispatchDomainEventsAsync<T>(this IMediator mediator, T t) where T : BaseEntity, IAggregateRoot
    {
        if (t.DomainEvents?.Any() ?? false)
        {
            var domainEvents = t.DomainEvents.ToList();

            t.ClearDomainEvents();

            foreach (var domainEvent in domainEvents)
                await mediator.Publish(domainEvent);
        }
    }
}
