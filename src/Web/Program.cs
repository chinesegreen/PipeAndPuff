using Infrastructure.Options;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Web.Configuration;
using Infrastructure.Services;
using Microsoft.AspNetCore.Http.HttpResults;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddConsole();

// Add services to the container.
builder.Services.AddMvc();

if (!builder.Environment.IsDevelopment() || builder.Environment.EnvironmentName == "Docker")
{
    Infrastructure.Dependencies.ConfigureServices(builder.Configuration, builder.Services);
}
else
{
    builder.Services.AddDbContext<AppIdentityDbContext>(options =>
    {
        var connectionString = builder.Configuration.GetConnectionString("IdentityConnection");
        options.UseNpgsql(connectionString, sqlOptions => sqlOptions.EnableRetryOnFailure());
    });
    builder.Services.AddDbContext<CatalogContext>(options =>
    {
        var connectionString = builder.Configuration.GetConnectionString("CatalogConnection");
        options.UseNpgsql(connectionString, sqlOptions => sqlOptions.EnableRetryOnFailure());
    });
}

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(50);
    options.Cookie.Name = ".Pipe&Puff.Session";
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Tokens.ProviderMap.Add("CustomEmailConfirmation",
        new TokenProviderDescriptor(
            typeof(CustomEmailConfirmationTokenProvider<ApplicationUser>)));
    options.Tokens.EmailConfirmationTokenProvider = "CustomEmailConfirmation";

}).AddEntityFrameworkStores<AppIdentityDbContext>().AddDefaultUI();

builder.Services.AddIdentityOptions();

builder.Services.AddCookieSettings();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();

builder.Services.AddPolicies();

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddCoreServices(builder.Configuration);
builder.Services.AddWebServices(builder.Configuration);

builder.Services.AddMemoryCache();

builder.Services.Configure<AuthMessageSenderOptions>(builder.Configuration);
builder.Services.Configure<AwsCredentials>(builder.Configuration);
builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
       options.TokenLifespan = TimeSpan.FromHours(3));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.EnvironmentName == "Docker")
{
    app.Logger.LogInformation("Adding Development middleware...");
    app.UseDeveloperExceptionPage();
}
else
{
    app.Logger.LogInformation("Adding non-Development middleware...");
    app.UseExceptionHandler("/Error");
    app.UseHsts();

    app.Map("/ping", (IApplicationBuilder branch) =>
    {
        branch.Run(async (HttpContext context) =>
        {
            context.Response.ContentType = "text/plain";
            await context.Response.WriteAsync("pong");
        });
    });
}

using (var scope = app.Services.CreateScope())
{
    var scopedProvider = scope.ServiceProvider;
    try
    {
        var userManager = scopedProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var identityContext = scopedProvider.GetRequiredService<AppIdentityDbContext>();
        await AppIdentityDbContextSeed.SeedAsync(identityContext, userManager);
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "An error occurred seeding the DB.");
    }
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.MapDefaultControllerRoute();
app.MapControllers();

app.MapRazorPages();

app.Run();
