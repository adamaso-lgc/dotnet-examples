using FieldUp.Domain.Enums;
using FieldUp.Domain.ValueObjects;

namespace FieldUp.Domain.UnitTests;

public class ReservationTests
{
    [Fact]
    public void Constructor_WithValidParameters_CreatesReservation()
    {
        // Arrange
        var reservationId = Guid.NewGuid();
        var customerName = new Name("John", "Doe");
        const string email = "john.doe@example.com";
        var start = DateTimeOffset.UtcNow;
        var end = start.AddHours(2);
        var range = new TimeRange(start, end);

        // Act
        var reservation = new Reservation(reservationId, customerName, email, range);

        // Assert
        Assert.Equal(reservationId, reservation.ReservationId);
        Assert.Equal(customerName, reservation.CustomerName);
        Assert.Equal(email, reservation.Email);
        Assert.Equal(range, reservation.Range);
        Assert.Equal(ReservationStatus.Reserved, reservation.Status);
    }

    [Fact]
    public void Constructor_WithEmptyGuid_ThrowsArgumentException()
    {
        // Arrange
        var customerName = new Name("John", "Doe");
        const string email = "john.doe@example.com";
        var range = new TimeRange(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddHours(2));

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => 
            new Reservation(Guid.Empty, customerName, email, range));
        Assert.Equal("reservationId", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithNullCustomerName_ThrowsArgumentNullException()
    {
        // Arrange
        var reservationId = Guid.NewGuid();
        const string email = "john.doe@example.com";
        var range = new TimeRange(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddHours(2));

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => 
            new Reservation(reservationId, null!, email, range));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WithInvalidEmail_ThrowsArgumentException(string? email)
    {
        // Arrange
        var reservationId = Guid.NewGuid();
        var customerName = new Name("John", "Doe");
        var range = new TimeRange(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddHours(2));

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => 
            new Reservation(reservationId, customerName, email!, range));
        Assert.Equal("email", exception.ParamName);
    }

    [Fact]
    public void Constructor_WithNullRange_ThrowsArgumentNullException()
    {
        // Arrange
        var reservationId = Guid.NewGuid();
        var customerName = new Name("John", "Doe");
        const string email = "john.doe@example.com";

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => 
            new Reservation(reservationId, customerName, email, null!));
    }

    [Fact]
    public void Constructor_SetsStatusToReserved()
    {
        // Arrange
        var reservationId = Guid.NewGuid();
        var customerName = new Name("John", "Doe");
        const string email = "john.doe@example.com";
        var range = new TimeRange(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddHours(2));

        // Act
        var reservation = new Reservation(reservationId, customerName, email, range);

        // Assert
        Assert.Equal(ReservationStatus.Reserved, reservation.Status);
    }

    [Fact]
    public void Cancel_ChangesStatusToCancelled()
    {
        // Arrange
        var reservationId = Guid.NewGuid();
        var customerName = new Name("John", "Doe");
        const string email = "john.doe@example.com";
        var range = new TimeRange(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddHours(2));
        var reservation = new Reservation(reservationId, customerName, email, range);

        // Act
        reservation.Cancel();

        // Assert
        Assert.Equal(ReservationStatus.Cancelled, reservation.Status);
    }

    [Fact]
    public void Cancel_WhenAlreadyCancelled_RemainsCancelled()
    {
        // Arrange
        var reservationId = Guid.NewGuid();
        var customerName = new Name("John", "Doe");
        const string email = "john.doe@example.com";
        var range = new TimeRange(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddHours(2));
        var reservation = new Reservation(reservationId, customerName, email, range);
        reservation.Cancel();

        // Act
        reservation.Cancel();

        // Assert
        Assert.Equal(ReservationStatus.Cancelled, reservation.Status);
    }
}