using Bison.Razor;
using Microsoft.AspNetCore.Identity;
using Bison.Infrastructure;

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
builder.Services.AddSingleton<IObservationService, ObservationService>();

Console.WriteLine(
    $"ClientId = '{builder.Configuration["authentication:github:clientId"]}'");

Console.WriteLine(
    $"ClientSecret exists = {!string.IsNullOrEmpty(
        builder.Configuration["authentication:github:clientSecret"])}");

builder.Services.AddAuthentication()
    .AddGitHub(o =>
    {
        o.ClientId     = builder.Configuration["authentication:github:clientId"]!;
        o.ClientSecret = builder.Configuration["authentication:github:clientSecret"]!;
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


try
{
    bool response = await Client.InitializeDatabase();
}
catch (Exception ex)
{
    app.Logger.LogWarning(ex, "Startup API call failed");
}

app.Run();
