using FieldUp.Api.Features.FieldSchedule.Create;
using FieldUp.Domain.ValueObjects;
using FieldUp.Infrastructure.Persistence;
using MediatR;

namespace FieldUp.Api.Features.Reservations.Create;

public class CreateReservationHandler(IEventRepository<Domain.FieldSchedule, string> eventRepository) 
    :  IRequestHandler<CreateReservationRequest, Guid>
{
    private readonly IEventRepository<Domain.FieldSchedule, string> _eventRepository = 
        eventRepository ?? throw  new ArgumentNullException(nameof(eventRepository));
    
    public async Task<Guid> Handle(CreateReservationRequest request, CancellationToken cancellationToken)
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

        return reservationId;
    }
}