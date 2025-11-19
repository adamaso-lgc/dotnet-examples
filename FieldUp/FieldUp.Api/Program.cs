using FieldUp.Api.Features.Reservations;
using FieldUp.Api.Features.Reservations.Create;
using FieldUp.Infrastructure;
using Scalar.AspNetCore;
using Wolverine;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseWolverine();

builder.Services.AddOpenApi();
builder.Services.AddValidation();
builder.Services.AddMediatR(cfg => 
    cfg.RegisterServicesFromAssembly(typeof(CreateReservationHandler).Assembly));
builder.Services.AddSecrets(builder.Environment);
builder.Services.AddMartenInfrastructure(builder.Configuration);
builder.Services.AddEmailService(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.MapReservationsEndpoints();

app.Run();
