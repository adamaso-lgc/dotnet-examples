namespace FieldUp.Domain.Events;

public record ReservationCreated(
    string FieldScheduleId,
    Guid ReservationId,
    string FirstName,
    string LastName,
    string Email,
    DateTimeOffset Start,
    DateTimeOffset End
);
