using Microsoft.EntityFrameworkCore;
using Mundial.Modelo;
using Mundial.Persistencia;
using Mundial.ViewModels;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Mundial_db");

var mysqlVersion = new MySqlServerVersion("8.0.30");

builder.Services.AddDbContext<MundialDbContext>(opcion =>
    opcion.UseMySql(connectionString, mysqlVersion));

builder.Services.AddDbContext<MundialDbContext>();

var opciones = new DbContextOptionsBuilder<MundialDbContext>();

opciones.UseMySql(connectionString, mysqlVersion);

var contexto = new MundialDbContext(opciones.Options);

contexto.Database.EnsureCreated();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("appPolicy",
                    builder =>
                    {
                        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("appPolicy");

app.MapPost("/paises/", async (MundialDbContext db, PaisViewModel pais) =>
{
    var nuevoPais = new Pais()
    {
        Nombre = pais.Nombre,
        Mundial = pais.Mundial,
        Color = ""
    };
    await db.Paises.AddAsync(nuevoPais);
    await db.SaveChangesAsync();
    return Results.Created($"/paises/{nuevoPais.Id}", nuevoPais);
});

app.MapGet("/paises/", async (MundialDbContext db) =>
{
    var Paises = await db.Paises.ToListAsync();
    var random = new Random();
    foreach (var pais in Paises)
    {
        pais.Color = String.Format("#{0:X6}", random.Next(0x1000000));
    }
    return Results.Ok(Paises);
});

// int CalcularNroInstancia(Pais p)
// {
//     switch (p.Instancia.ToUpper())
//     {
//         case "NO PARTICIPO": { return 0; }
//         case "FASE DE GRUPOS": { return 1; }
//         case "OCTAVOS DE FINAL": { return 2; }
//         case "CUARTOS DE FINAL": { return 3; }
//         case "SEMIFINAL": { return 4; }
//         case "TERCER PUESTO": { return 5; }
//         case "SUBCAMPEÓN": { return ; }
//         case "CAMPEÓN": { return 1; }
//     }
//     return 0;
// }

app.Run();