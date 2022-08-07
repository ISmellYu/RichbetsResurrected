using Autofac;
using MediatR;
using RichbetsResurrected.Communication.Slots.Events;
using RichbetsResurrected.Entities.Games.Slots;
using RichbetsResurrected.Interfaces.DAL;
using RichbetsResurrected.Interfaces.Utils;

namespace RichbetsResurrected.Services.Utils;

public class BackgroundTasks : IBackgroundTasks
{
    private readonly ILifetimeScope _lifetimeScope;
    public BackgroundTasks(ILifetimeScope lifetimeScope)
    {
        _lifetimeScope = lifetimeScope;
    }
    
    public async Task DelayWithdrawalAsync(SlotsWithdrawResult result, int delay, int userId, string connectionId)
    {
        await Task.Delay(delay);

        await using var scope = _lifetimeScope.BeginLifetimeScope();
        var mediator = scope.Resolve<IMediator>();
        await mediator.Publish(new SlotsWithdrawNotification(result, connectionId));
        if (!result.IsWin || result.WinAmount is null)
            return;
        var richbetRepository = scope.Resolve<IRichbetRepository>();
        await richbetRepository.AddPointsToUserAsync(userId, result.WinAmount.Value);
    }
}