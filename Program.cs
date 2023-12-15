using Analiza_Risc.Data;
using Analiza_Risc.Models;
using Analiza_Risc.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;


var builder = WebApplication.CreateBuilder(args);

// Adaugă serviciile la container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddRazorPages();
builder.Services.AddScoped<IServiceCRUD, ServiceCRUD>();

builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseNpgsql(builder.Configuration.GetConnectionString("PostgresSQL"));
});

// builder.Services.AddDbContext<AnalizaRisccContext>(opt =>
// {
//     opt.UseNpgsql(builder.Configuration.GetConnectionString("PostgresSQL"));
// });


var app = builder.Build();

// Configurează pipeline-ul de cereri HTTP.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Adaugă middleware-ul de autentificare.
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();
app.Run();
