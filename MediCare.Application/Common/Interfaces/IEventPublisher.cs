namespace MediCare.Application.Common.Interfaces;

public interface IEventPublisher
{
    Task PublishAsync(string topic, object? content, CancellationToken cancellationToken = default);
}
