using FieldUp.Domain.Core;
using FieldUp.Domain.Events;
using FieldUp.Domain.ValueObjects;

namespace FieldUp.Domain;

public class FieldSchedule : AggregateRoot<string>
{
    private readonly List<Reservation> _reservations = [];
    public IReadOnlyList<Reservation> Reservations => _reservations;
    public Guid FieldId { get; private set; }
    public DateOnly Date { get; private set; }
    
    public FieldSchedule() {}
    
    public static FieldSchedule Create(Guid fieldId, DateOnly date)
    {
        var schedule = new FieldSchedule();
        schedule.AddEvent(new FieldScheduleCreated(fieldId, date));
        return schedule;
    }
    
    public void Reserve(Guid reservationId, Name name, string email, TimeRange range)
    {
        if (_reservations.Any(r => r.Range.Overlaps(range)))
            throw new InvalidOperationException("Time slot already taken.");

        AddEvent(new ReservationCreated(
            Id,
            reservationId,
            name.FirstName,
            name.LastName,
            email,
            range.Start,
            range.End
        ));
    }
    
    public void Apply(FieldScheduleCreated e)
    {
        FieldId = e.FieldId;
        Date = e.Date;
        Id = $"{e.FieldId}:{e.Date:yyyy-MM-dd}";
    }

    public void Apply(ReservationCreated e)
    {
        _reservations.Add(new Reservation(
            e.ReservationId,
            new Name(e.FirstName, e.LastName),
            e.Email,
            new TimeRange(e.Start, e.End)
        ));
    }
    
    
}