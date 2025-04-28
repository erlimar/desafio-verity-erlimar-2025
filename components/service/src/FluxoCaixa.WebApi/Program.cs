using FluxoCaixa.WebApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails();

builder.Services.AddOpenApi();

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/openapi/v1.json", "Fluxo Caixa API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.MapUserMeEndpoint();

app.Run();

