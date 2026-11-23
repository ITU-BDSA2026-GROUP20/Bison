using Bison.Razor;
using Microsoft.AspNetCore.Identity;
using Bison.Infrastructure;
using Microsoft.AspNetCore.Authentication.Cookies;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
string dbPath = Environment.GetEnvironmentVariable("BISONDBPATH")
    ?? Path.Combine(Path.GetTempPath(), "bison.db");

builder.Services.AddBisonInfrastructure($"Data Source={dbPath}");

// Add services to the container.
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>  
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
})
.AddEntityFrameworkStores<Database>();
builder.Services.AddRazorPages();
builder.Services.AddScoped<IObservationService, ObservationService>();

var githubClientId     = builder.Configuration["github:clientid"];
var githubClientSecret = builder.Configuration["github:clientsecret"];
var githubConfigured = !string.IsNullOrWhiteSpace(githubClientId)
    && !string.IsNullOrWhiteSpace(githubClientSecret);


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    if (githubConfigured)
        options.DefaultChallengeScheme = "GitHub";
})
.AddCookie()
.AddGitHub(o =>
{
    o.ClientId     = githubClientId!;
    o.ClientSecret = githubClientSecret!;
    o.CallbackPath = "/signin-github";
    o.Scope.Add("user:email");
});
    
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    var initializer = scope.ServiceProvider
        .GetRequiredService<DatabaseInitializer>();

    await initializer.InitializeAsync();
}

app.Run();