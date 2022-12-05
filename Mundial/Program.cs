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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/paises/", async (MundialDbContext db, PaisViewModel pais) =>
{
    var nuevoPais = new Pais()
    {
        Nombre = pais.Nombre,
    };
    await db.Paises.AddAsync(nuevoPais);
    await db.SaveChangesAsync();
    return Results.Created($"/paises/{nuevoPais.Id}", nuevoPais);
});

app.MapGet("/paises/", async (MundialDbContext db) =>
{
    return Results.Ok(await db.Paises.Include(x => x.Participaciones).ToListAsync());
});


app.MapPost("/paises/{nombre}/participaciones", async (MundialDbContext db, string nombre, List<ParticipacionViewModel> participaciones) =>
{
    var pais = await db.Paises.FirstOrDefaultAsync(x => x.Nombre == nombre);
    var participacionesAdicionales = new List<Participacion>();
    foreach (var participacion in participaciones)
    {
        participacionesAdicionales.Add(new Participacion
        {
            Sede = participacion.Sede,
            Año = participacion.Año,
            Instancia = participacion.Instancia
        });
    }
    pais.Participaciones.AddRange(participacionesAdicionales);
    db.Entry(pais).State = EntityState.Modified;
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapGet("/paises/{nombre}/participaciones", (MundialDbContext db, string nombre) =>
{
    var pais = db.Paises.Where(x => x.Nombre == nombre, i => i.include(x => x.Participacion));

    return Results.Ok(pais);
});

app.Run();