using FieldUp.Api.Features.Reservations.Create;
using MediatR;

namespace FieldUp.Api.Features.Reservations;

public static class ReservationsEndpoint
{
    public static IEndpointRouteBuilder MapReservationsEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/reservations")
            .WithTags("Reservations");
            //.WithOpenApi();

        group.MapPost("/", async (CreateReservationRequest request, IMediator mediator, CancellationToken ct) =>
            {
                var reservationId = await mediator.Send(request, ct);

                return Results.Created($"/reservations/{reservationId}", new { ReservationId = reservationId });
            })
            .WithName("CreateReservation")
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest);

        return routes;
    }
}