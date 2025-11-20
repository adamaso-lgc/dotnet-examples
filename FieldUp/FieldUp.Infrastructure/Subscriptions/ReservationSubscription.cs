using JasperFx.Core.Reflection;
using JasperFx.Events.Daemon;
using JasperFx.Events.Projections;
using Marten;
using Marten.Subscriptions;
using Microsoft.Extensions.Logging;

namespace FieldUp.Infrastructure.Subscriptions;

public class ReservationSubscription : ISubscription
{
    public Task<IChangeListener> ProcessEventsAsync(EventRange page, ISubscriptionController controller, IDocumentOperations operations,
        CancellationToken cancellationToken)
    {
        Console.WriteLine($"Starting to process events from {page.SequenceFloor} to {page.SequenceCeiling}");
        foreach (var e in page.Events)
        {
            Console.WriteLine($"Got event of type {e.Data.GetType().NameInCode()} from stream {e.StreamId}");
        }

        return Task.FromResult(NullChangeListener.Instance);
    }
}