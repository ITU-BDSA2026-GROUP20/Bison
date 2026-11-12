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
    options.SignIn.RequireConfirmedAccount =true;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
})
.AddEntityFrameworkStores<Database>();
builder.Services.AddRazorPages();
builder.Services.AddSingleton<IObservationService, ObservationService>();

ConfigureServices(builder.Services);
    
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

Configure(app);

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
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


void ConfigureServices(IServiceCollection services)
{
    services.AddAuthentication(options => { /* Authentication options */ })
            .AddGitHub(options =>
            {
                options.ClientId = "49e302895d8b09ea5656";
                options.ClientSecret = "98f1bf028608901e9df91d64ee61536fe562064b";
            });
}


void Configure(IApplicationBuilder app)
{
    app.UseAuthentication();
    app.UseAuthorization();
}
