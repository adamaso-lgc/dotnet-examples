using FieldUp.Domain.Events;
using FieldUp.Domain.ValueObjects;
using Marten.Events.Aggregation;

namespace FieldUp.Infrastructure.Projections;

public class ReservationViewProjection : SingleStreamProjection<ReservationView, Guid>
{
    public void Apply(ReservationView view, ReservationCreated e)
    {
        view.Id = e.ReservationId.ToString();
        view.FieldId = e.FieldId;
        view.Start = e.Start;
        view.End = e.End;
        view.FirstName = e.FirstName;
        view.LastName = e.LastName;
        view.Email = e.Email;
        view.Status = nameof(ReservationStatus.Reserved);
    }
}