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
    var pais = db.Paises.Where(x => x.Nombre == nombre).Include(x => x.Participaciones);

    return Results.Ok(pais);
});

app.MapGet("/grafico/participaciones", (MundialDbContext db) =>
{
    var paisesParticipaciones = db.Paises.Include(x => x.Participaciones);

    var resultadoPaises = new List<PaisParticipacionesViewModel>();
    var resultadoParticipaciones = new List<ParticipacionDtoViewModel>();

    foreach (var pais in paisesParticipaciones)
    {
        resultadoParticipaciones.Clear();
        foreach (var p in pais.Participaciones)
        {
            int nroInstancia = CalcularNroInstancia(p);
            resultadoParticipaciones.Add(new ParticipacionDtoViewModel()
            {
                Id = p.Id,
                Sede = p.Sede,
                Año = p.Año,
                Instancia = p.Instancia,
                NroInstancia = nroInstancia
            });
        }
        resultadoPaises.Add(new PaisParticipacionesViewModel
        {
            Id = pais.Id,
            Nombre = pais.Nombre,
            Participaciones = resultadoParticipaciones.Select(x => x).ToList()
        });
    }

    return Results.Ok(resultadoPaises);
});


int CalcularNroInstancia(Participacion p)
{
    switch (p.Instancia.ToUpper())
    {
        case "NO PARTICIPO": { return 0; }
        case "FASE DE GRUPOS": { return 1; }
        case "OCTAVOS DE FINAL": { return 2; }
        case "CUARTOS DE FINAL": { return 3; }
        case "SEMIFINAL": { return 4; }
        case "TERCER PUESTO": { return 5; }
        case "SUBCAMPEÓN": { return 6; }
        case "CAMPEÓN": { return 7; }
    }

    return 0;
}

app.MapGet("/grafico/años", (MundialDbContext db) =>
{
    var añosMundial = db.Participaciones.Select(x => x.Año).ToList();
    var resultadoAños = añosMundial.Distinct();

    return Results.Ok(resultadoAños);

});

app.Run();