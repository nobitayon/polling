using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;

namespace Delta.Polling.WebAPI.Infrastructure.Documentation;

public static class ConfigureDocumentation
{
    public static void AddDocumentationService(this IServiceCollection services, IConfiguration configuration)
    {
        var documentationOptions = configuration.GetSection(DocumentationOptions.SectionKey).Get<DocumentationOptions>()
            ?? throw new ConfigurationBindingFailedException(DocumentationOptions.SectionKey, typeof(DocumentationOptions));

        _ = services.AddSwaggerGen(options =>
        {
            options.CustomSchemaIds(type =>
            {
                return type.FullName;
            });

            options.SwaggerDoc(documentationOptions.Version, new OpenApiInfo
            {
                Title = documentationOptions.Title,
                Version = documentationOptions.Version
            });

            options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
            {
                Name = HeaderNames.Authorization,
                Description = $"{JwtConstants.TokenType} {HeaderNames.Authorization} header using the {JwtBearerDefaults.AuthenticationScheme} scheme.",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Id = JwtBearerDefaults.AuthenticationScheme,
                            Type = ReferenceType.SecurityScheme
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });
    }

    public static IApplicationBuilder UseDocumentationService(this WebApplication app)
    {
        _ = app.UseSwagger();
        _ = app.UseSwaggerUI();

        return app;
    }
}
