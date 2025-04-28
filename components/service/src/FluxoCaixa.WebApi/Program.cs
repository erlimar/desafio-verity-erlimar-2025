using FluxoCaixa.DatabaseAccess;
using FluxoCaixa.KeycloakGateway;
using FluxoCaixa.RabbitMQGateway;
using FluxoCaixa.WebApi;
using FluxoCaixa.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails();
builder.Services.AddOpenIdConnectAuthorization(builder.Configuration);
builder.Services.AddApplicationUseCases();
builder.Services.AddKeycloakUserIdentityGateway();
builder.Services.AddRabbitMQMessageBrokerGateway();

builder.Services.AddMongoClient(
    connectionStringName: "MongoDB",
    databaseName: "fluxocaixa");

builder.Services.AddMongoDBRepositories();

var app = builder.Build();

app.UseExceptionHandler();

app.UseOpenIdConnectAuthorization();

app.MapGroup("/lancamentos")
    .WithTags("Lançamentos")
    .MapRegistrarLancamento();

app.Run();