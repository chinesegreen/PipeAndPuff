namespace Web.Configuration
{
    public static class ConfigurePolicies
    {
        public static IServiceCollection AddPolicies(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(
                    "Admin",
                    policyBuilder => policyBuilder
                        .RequireClaim("IsAdmin"));
            });

            return services;
        }
    }
}
