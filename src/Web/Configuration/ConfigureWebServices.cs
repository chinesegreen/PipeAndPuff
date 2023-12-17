using Infrastructure.Identity;
using Infrastructure.Interfaces;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Session;
using System.Reflection;

namespace Web.Configuration;

public static class ConfigureWebServices
{
    public static IServiceCollection AddWebServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddTransient<IEmailSender, EmailSender>();
        services.AddTransient<CustomEmailConfirmationTokenProvider<ApplicationUser>>();
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
        });

        services.AddScoped<ILocalStorageService, LocalStorageService>();

        return services;
    }
}
