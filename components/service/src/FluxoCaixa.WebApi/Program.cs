using FluxoCaixa.WebApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails();
builder.Services.AddOpenIdConnectAuthorization(builder.Configuration);

var app = builder.Build();

app.UseExceptionHandler();

app.UseOpenIdConnectAuthorization();

app.MapGroup("/lancamentos")
    .WithTags("Lançamentos")
    .MapRegistrarLancamento();

app.Run();