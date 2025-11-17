using FieldUp.Domain.ValueObjects;
using FieldUp.Infrastructure.Persistence;
using MediatR;

namespace FieldUp.Api.Features.FieldSchedule;

public record CreateReservationRequest : IRequest
{
    public Guid FieldId { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string Email { get; init; }
    public DateTime Start { get; init; }
    public DateTimeOffset End{ get; init; }
}

public class CreateReservation(IEventRepository<Domain.FieldSchedule, string> eventRepository) 
    :  IRequestHandler<CreateReservationRequest>
{
    private readonly IEventRepository<Domain.FieldSchedule, string> _eventRepository = 
        eventRepository ?? throw  new ArgumentNullException(nameof(eventRepository));
    
    public async Task Handle(CreateReservationRequest request, CancellationToken cancellationToken)
    {
        var date = DateOnly.FromDateTime(request.Start);
        var scheduleId = $"{request.FieldId:N}:{date:yyyy-MM-dd}";
        
        var schedule = await _eventRepository.GetAsync(scheduleId, cancellationToken)
                       ?? Domain.FieldSchedule.Create(scheduleId, request.FieldId, date);

        var reservationId = Guid.NewGuid();
        
        schedule.CreateReservation(
            reservationId,
            new Name(request.FirstName, request.LastName),
            request.Email,
            new TimeRange(request.Start, request.End)
        );
        
        await _eventRepository.SaveAsync(schedule, cancellationToken);
    }
}