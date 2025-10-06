using BancoTurnosApp.src.services;
using BancoTurnosApp.src.Data;
using BancoTurnosApp.src.repositories;
using BancoTurnosApp.src.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Controladores (MVC)
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// Inyección de dependencias - CLIENTES
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IClienteService, ClienteService>();

// Inyección de dependencias - SERVICIOS
builder.Services.AddScoped<IServicioRepository, ServicioRepository>();
builder.Services.AddScoped<IServicioService, ServicioService>();

// Inyección de dependencias - TURNOS
builder.Services.AddScoped<ITurnoRepository, TurnoRepository>();
builder.Services.AddScoped<ITurnoService, TurnoService>();

var app = builder.Build();

// ✨ CREAR BASE DE DATOS AUTOMÁTICAMENTE ✨
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        
        // Esto crea la base de datos y las tablas si no existen
        context.Database.EnsureCreated();
        
        Console.WriteLine("✅ Base de datos creada/verificada exitosamente");
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "❌ Error al crear la base de datos");
        // No lanzamos la excepción para que la app intente continuar
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

// Rutas de controladores
app.MapControllers();

app.Run();