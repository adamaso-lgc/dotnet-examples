using System.ComponentModel.DataAnnotations;
using MediatR;

namespace FieldUp.Api.Features.Reservations.Create;

public record CreateReservationRequest : IRequest<Guid>
{
    [Required]
    public Guid FieldId { get; init; }
    
    [Required]
    public required string FirstName { get; init; }
    
    [Required]
    public required string LastName { get; init; }
    
    [Required, EmailAddress]
    public required string Email { get; init; }
    
    [Required]
    public DateTime Start { get; init; }
    
    [Required]
    public DateTimeOffset End{ get; init; }
}