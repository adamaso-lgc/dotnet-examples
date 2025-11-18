using FieldUp.Domain.Events;
using FieldUp.Infrastructure.Services;
using Wolverine.Attributes;

namespace FieldUp.Api.Features.Reservations.Create;

[WolverineHandler]
public class ReservationCreatedHandler(
    IEmailService emailService,
    ILogger<ReservationCreatedHandler> logger,
    IConfiguration configuration)
{
    private readonly IEmailService _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
    private readonly ILogger<ReservationCreatedHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly IConfiguration _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    
    public async Task Handle(ReservationCreated message, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Sending confirmation email for reservation {Id}", message.ReservationId);

        // Build cancel URL
        var frontendBaseUrl = _configuration["FrontendBaseUrl"]; 
        var cancelUrl = $"{frontendBaseUrl}/reservations/cancel/{message.ReservationId}";

        const string subject = "Your Reservation is Confirmed!";
        var body = $"""

                                <p>Hi {message.FirstName},</p>
                                <p>Your reservation is confirmed:</p>
                                <ul>
                                    <li>Field: {message.FieldId}</li>
                                    <li>Start: {message.Start}</li>
                                    <li>End: {message.End}</li>
                                </ul>
                                <p>If you need to cancel, please click <a href='{cancelUrl}'>here</a>.</p>
                                <p>Thanks!</p>
                            
                    """;

        await _emailService.SendEmailAsync(message.Email, subject, body, cancellationToken);
    }
}