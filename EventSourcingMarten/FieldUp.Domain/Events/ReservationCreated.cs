namespace FieldUp.Domain.Events;

public record ReservationCreated(
    Guid ReservationId,
    Guid FieldId,
    string FirstName,
    string LastName,
    string Email,
    DateTimeOffset Start,
    DateTimeOffset End
);
