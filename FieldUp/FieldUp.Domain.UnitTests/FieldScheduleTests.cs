using FieldUp.Domain.ValueObjects;

namespace FieldUp.Domain.UnitTests;

public class FieldScheduleTests
{
    [Fact]
    public void Create_WithValidParameters_CreatesFieldSchedule()
    {
        // Arrange
        var fieldId = Guid.NewGuid();
        var date = new DateOnly(2024, 1, 15);

        // Act
        var schedule = FieldSchedule.Create(fieldId, date);

        // Assert
        Assert.NotNull(schedule);
        Assert.Equal(1, schedule.Version);
        Assert.Equal(fieldId, schedule.FieldId);
        Assert.Equal(date, schedule.Date);
        Assert.Equal($"{fieldId}:{date:yyyy-MM-dd}", schedule.Id);
        Assert.Empty(schedule.Reservations);
    }
    
    
    [Fact]
    public void Reserve_WithValidTimeRange_AddsReservation()
    {
        // Arrange
        var fieldId = Guid.NewGuid();
        var date = new DateOnly(2024, 1, 15);
        var schedule = FieldSchedule.Create(fieldId, date);

        var reservationId = Guid.NewGuid();
        var name = new Name("John", "Doe");
        const string email = "john.doe@example.com";
        var start = new DateTimeOffset(2024, 1, 15, 10, 0, 0, TimeSpan.Zero);
        var end = start.AddHours(2);
        var range = new TimeRange(start, end);

        // Act
        schedule.Reserve(reservationId, name, email, range);

        // Assert
        Assert.Single(schedule.Reservations);
        Assert.Equal(2, schedule.Version);
        var reservation = schedule.Reservations[0];
        Assert.Equal(reservationId, reservation.ReservationId);
        Assert.Equal(name, reservation.CustomerName);
        Assert.Equal(email, reservation.Email);
        Assert.Equal(range, reservation.Range);
    }

    [Fact]
    public void Reserve_WithOverlappingTimeRange_ThrowsInvalidOperationException()
    {
        // Arrange
        var fieldId = Guid.NewGuid();
        var date = new DateOnly(2024, 1, 15);
        var schedule = FieldSchedule.Create(fieldId, date);

        var start = new DateTimeOffset(2024, 1, 15, 10, 0, 0, TimeSpan.Zero);
        var end = start.AddHours(2);
        var range1 = new TimeRange(start, end);

        schedule.Reserve(Guid.NewGuid(), new Name("John", "Doe"), "john@example.com", range1);

        var overlappingStart = start.AddMinutes(30);
        var overlappingEnd = overlappingStart.AddHours(2);
        var range2 = new TimeRange(overlappingStart, overlappingEnd);

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() =>
            schedule.Reserve(Guid.NewGuid(), new Name("Jane", "Smith"), "jane@example.com", range2));
        Assert.Equal("Time slot already taken.", exception.Message);
    }

    [Theory]
    [InlineData(8, 10, 10, 12)]   // Adjacent slots
    [InlineData(8, 10, 11, 13)]   // Non-overlapping with gap
    [InlineData(10, 12, 8, 10)]   // Adjacent before
    public void Reserve_WithNonOverlappingTimeRange_AddsReservation(int start1Hour, int end1Hour, int start2Hour, int end2Hour)
    {
        // Arrange
        var fieldId = Guid.NewGuid();
        var date = new DateOnly(2024, 1, 15);
        var schedule = FieldSchedule.Create(fieldId, date);

        var baseDate = new DateTimeOffset(2024, 1, 15, 0, 0, 0, TimeSpan.Zero);
        var range1 = new TimeRange(baseDate.AddHours(start1Hour), baseDate.AddHours(end1Hour));
        var range2 = new TimeRange(baseDate.AddHours(start2Hour), baseDate.AddHours(end2Hour));

        schedule.Reserve(Guid.NewGuid(), new Name("John", "Doe"), "john@example.com", range1);

        // Act
        schedule.Reserve(Guid.NewGuid(), new Name("Jane", "Smith"), "jane@example.com", range2);

        // Assert
        Assert.Equal(2, schedule.Reservations.Count);
    }

    [Fact]
    public void Reservations_IsReadOnly()
    {
        // Arrange
        var fieldId = Guid.NewGuid();
        var date = new DateOnly(2024, 1, 15);
        var schedule = FieldSchedule.Create(fieldId, date);

        // Act
        var reservations = schedule.Reservations;

        // Assert
        Assert.IsType<IReadOnlyList<Reservation>>(reservations, exactMatch: false);
    }

    [Fact]
    public void Reserve_MultipleReservations_AddsAllSuccessfully()
    {
        // Arrange
        var fieldId = Guid.NewGuid();
        var date = new DateOnly(2024, 1, 15);
        var schedule = FieldSchedule.Create(fieldId, date);

        var baseDate = new DateTimeOffset(2024, 1, 15, 0, 0, 0, TimeSpan.Zero);
        var range1 = new TimeRange(baseDate.AddHours(8), baseDate.AddHours(10));
        var range2 = new TimeRange(baseDate.AddHours(10), baseDate.AddHours(12));
        var range3 = new TimeRange(baseDate.AddHours(14), baseDate.AddHours(16));

        // Act
        schedule.Reserve(Guid.NewGuid(), new Name("John", "Doe"), "john@example.com", range1);
        schedule.Reserve(Guid.NewGuid(), new Name("Jane", "Smith"), "jane@example.com", range2);
        schedule.Reserve(Guid.NewGuid(), new Name("Bob", "Johnson"), "bob@example.com", range3);

        // Assert
        Assert.Equal(3, schedule.Reservations.Count);
    }

    [Fact]
    public void Reserve_WithCancelledReservationInSameSlot_AllowsNewReservation()
    {
        // Arrange
        var fieldId = Guid.NewGuid();
        var date = new DateOnly(2024, 1, 15);
        var schedule = FieldSchedule.Create(fieldId, date);

        var start = new DateTimeOffset(2024, 1, 15, 10, 0, 0, TimeSpan.Zero);
        var end = start.AddHours(2);
        var range = new TimeRange(start, end);

        schedule.Reserve(Guid.NewGuid(), new Name("John", "Doe"), "john@example.com", range);
        schedule.Reservations[0].Cancel();

        // Act
        schedule.Reserve(Guid.NewGuid(), new Name("Jane", "Smith"), "jane@example.com", range);

        // Assert
        Assert.Equal(2, schedule.Reservations.Count);
    }
}