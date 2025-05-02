//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.IdentityModel.Tokens;

//namespace Delta.Polling.Infrastructure.Authentication.Another;

//public static class ConfigureAnotherAuthentication
//{
//    public static IServiceCollection AddAnotherAuthentication(this IServiceCollection services, IConfiguration configuration)
//    {
//        var anotherAuthenticationOptions = configuration.GetSection(AnotherAuthenticationOptions.SectionKey).Get<AnotherAuthenticationOptions>()
//            ?? throw new ConfigurationBindingFailedException(AnotherAuthenticationOptions.SectionKey, typeof(AnotherAuthenticationOptions));

//        //_ = services.AddScoped<CustomJwtBearerEvents>();
//        //_ = services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//        //    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
//        //    {
//        //        options.Authority = anotherAuthenticationOptions.AuthorityUrl;
//        //        options.Audience = anotherAuthenticationOptions.Audience;

//        //        options.TokenValidationParameters = new TokenValidationParameters
//        //        {
//        //            ValidateIssuer = true,
//        //            ValidateAudience = true,
//        //            ClockSkew = TimeSpan.Zero
//        //        };

//        //        options.EventsType = typeof(CustomJwtBearerEvents);
//        //    });

//        _ = services.AddAuthentication(options =>
//        {
//            options.DefaultScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
//        });

//        // Register the OpenIddict validation components.
//        _ = services.AddOpenIddict()
//            .AddValidation(options =>
//            {
//                // Note: the validation handler uses OpenID Connect discovery
//                // to retrieve the address of the introspection endpoint.
//                options.SetIssuer("https://localhost:44395/");
//                options.AddAudiences("rs_dataEventRecordsApi");

//                // Configure the validation handler to use introspection and register the client
//                // credentials used when communicating with the remote introspection endpoint.
//                options.UseIntrospection()
//                        .SetClientId("rs_dataEventRecordsApi")
//                        .SetClientSecret("dataEventRecordsSecret");

//                //options.UseIntrospection()
//                //        .SetClientId("PollingRP")
//                //        .SetClientSecret("Secret222");

//                // Register the System.Net.Http integration.
//                options.UseSystemNetHttp();

//                // Register the ASP.NET Core host.
//                options.UseAspNetCore();
//            });

//        return services;
//    }
//}
