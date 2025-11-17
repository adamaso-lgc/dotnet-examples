namespace FieldUp.Domain.Events;

public record FieldScheduleCreated(
    Guid FieldId,
    DateOnly Date
);