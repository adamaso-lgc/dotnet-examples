using FieldUp.Domain.ValueObjects;

namespace FieldUp.Domain;

public class Reservation
{
    public Guid ReservationId { get; private set; }
    public Name CustomerName { get; private set; }
    public string Email { get; private set; }
    public TimeRange Range { get; private set; }
    public ReservationStatus Status { get; private set; }
    
    public Reservation(Guid reservationId, Name customerName, string email, TimeRange range)
    {
        if (reservationId == Guid.Empty)
            throw new ArgumentException("Reservation ID cannot be empty.", nameof(reservationId));

        ArgumentNullException.ThrowIfNull(customerName);

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty.", nameof(email));

        ArgumentNullException.ThrowIfNull(range);
        
        ReservationId = reservationId;
        CustomerName = customerName;
        Email = email;
        Range = range;
        Status = ReservationStatus.Reserved;
    }
    
    public void Cancel() => Status = ReservationStatus.Cancelled;
    
}