using BlazorAuth.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.ConfigureApplicationServices();

var app = builder.Build();
app.ConfigurePipeline();
app.Run();