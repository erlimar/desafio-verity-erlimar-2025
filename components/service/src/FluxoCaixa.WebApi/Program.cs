using FluxoCaixa.WebApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails();
builder.Services.AddOpenIdConnectAuthorization(builder.Configuration);

var app = builder.Build();

app.UseExceptionHandler();

app.UseOpenIdConnectAuthorization();

app.MapUserMeEndpoint();

app.Run();