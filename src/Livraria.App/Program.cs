using Livraria.App.Configurations;
using Livraria.Data.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", true, true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
    .AddEnvironmentVariables();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityConfiguration(builder.Configuration);

builder.Services.AddDbContext<LivrariaDbContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version())));

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddMvcConfiguration();

builder.Services.ResolveDependencies();

// Gerando a APP
var app = builder.Build();

// Configura��o conforme os ambientes
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/erro/500");
    app.UseStatusCodePagesWithRedirects("/erro/{0}");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseGlobalizationConfig();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
