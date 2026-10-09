using DotNetCore.CAP;
using MediCare.Application.Common.Interfaces;

namespace MediCare.Persistence.Messaging;

public class CapEventPublisher(ICapPublisher capPublisher) : IEventPublisher
{
    public async Task PublishAsync(string topic, object? content, CancellationToken cancellationToken = default)
    {
        await capPublisher.PublishAsync(topic, content, cancellationToken: cancellationToken);
    }
}
