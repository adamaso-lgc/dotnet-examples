namespace FieldUp.Domain.Events;

public record FieldScheduleCreated(
    string Id,
    Guid FieldId,
    DateOnly Date
);