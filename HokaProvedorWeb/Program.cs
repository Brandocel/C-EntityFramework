using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using HokaProvedorWeb.Data;
using HokaProvedorWeb.Interfaces;
using HokaProvedorWeb.Repositories;
using HokaProvedorWeb.Services;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios MVC (controladores y vistas)
builder.Services.AddControllersWithViews();

// Configuración de la base de datos
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrar repositorios y servicios
builder.Services.AddScoped<IProveedorRepository, ProveedorRepository>();
builder.Services.AddScoped<IProveedorService, ProveedorService>();

var app = builder.Build();

// Manejo de errores en producción
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Middleware
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Configuración de rutas MVC
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"); // Configuración para controladores y vistas

app.Run();
