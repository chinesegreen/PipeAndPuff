using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public static class Dependencies
    {
        public static void ConfigureServices(IConfiguration configuration, IServiceCollection services)
        {
            bool useOnlyInMemoryDatabase = false;
            if (configuration["UseOnlyInMemoryDatabase"] != null)
            {
                useOnlyInMemoryDatabase = bool.Parse(configuration["UseOnlyInMemoryDatabase"]!);
            }

            if (useOnlyInMemoryDatabase)
            {
                services.AddDbContext<CatalogContext>(options =>
                   options.UseInMemoryDatabase("Catalog"));
                services.AddDbContext<AppIdentityDbContext>(options =>
                    options.UseInMemoryDatabase("Identity"));
            }
            else
            {
                services.AddDbContext<CatalogContext>(options =>
                    options.UseNpgsql(configuration.GetConnectionString("CatalogConnection"), sqlOptions => sqlOptions.EnableRetryOnFailure()));
                services.AddDbContext<AppIdentityDbContext>(options =>
                    options.UseNpgsql(configuration.GetConnectionString("IdentityConnection"), sqlOptions => sqlOptions.EnableRetryOnFailure()));
            }
        }
    }
}
