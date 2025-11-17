namespace FieldUp.Infrastructure.Projections;

public class ReservationView
{
    public Guid ReservationId { get; set; }
    public Guid FieldId { get; set; }
    public DateTimeOffset Start { get; set; }
    public DateTimeOffset End { get; set; }
    public required string FirstName { get; set; } 
    public required string LastName { get; set; }
    public string CustomerFullName => $"{FirstName} {LastName}";
    public required string Email { get; set; }
    public required string Status { get; set; }
    public TimeSpan Duration => End - Start;
}