using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace FluxoCaixa.WebApi.Extensions;

/// <summary>
/// Métodos para configurar serviços de autenticação e autorização
/// </summary>
public static class ApplicationAuthExtensions
{
    /// <summary>
    /// Adiciona suporte a OpenID Connect com OpenApi e Swagger
    /// </summary>
    public static void AddOpenIdConnectAuthorization(this IServiceCollection services, IConfiguration configuration)
    {
        var openIdConnectUrl = configuration["Auth:OpenIdConnectUrl"];
        var metadataAddress = configuration["Auth:MetadataAddress"];
        var issuer = configuration["Auth:Issuer"];
        var audience = configuration["Auth:Audience"];

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            var securityScheme = new OpenApiSecurityScheme
            {
                Name = "OIDC",
                Description = "Autenticação usando OpenId Connect",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.OpenIdConnect,
                OpenIdConnectUrl = new Uri(openIdConnectUrl!),
                Scheme = "Bearer",
                BearerFormat = "JWT",
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
            };

            c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                [securityScheme] = ["Bearer"]
            });
        });
        services.AddAuthorization();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.MetadataAddress = metadataAddress!;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                };
            });
    }

    /// <summary>
    /// Configura o uso do OpenId Connect e Swagger no pipeline de requisições
    /// </summary>
    public static void UseOpenIdConnectAuthorization(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                c.RoutePrefix = string.Empty;
            });
        }
    }
}