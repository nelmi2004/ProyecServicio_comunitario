using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using ProyecServicio_comunitario.Models;
using ProyecServicio_comunitario.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Configurar PostgreSQL
builder.Services.AddDbContext<AngelDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresDb")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.OpenApiInfo 
    { 
        Title = "AngelDB API", 
        Version = "v1" 
    });
    //incluir comentarios XML para mejorar la documentación de Swagger
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services.AddScoped<VehicleService>();
builder.Services.AddScoped<PersonalService>();
builder.Services.AddScoped<GrupoService>();
builder.Services.AddScoped<HerramientasEquipoService>();
builder.Services.AddScoped<CategoriaService>();
builder.Services.AddScoped<CentrosSaludService>();
builder.Services.AddScoped<AutopistaService>();
builder.Services.AddScoped<EstatusService>();
builder.Services.AddScoped<OrganismoService>();
builder.Services.AddScoped<PersonalGrupoService>();
builder.Services.AddScoped<CompanyService>();
builder.Services.AddScoped<TrasladosEventoService>();
builder.Services.AddScoped<InvolucradosEventoService>();
builder.Services.AddScoped<LocalizacionEventoService>();
builder.Services.AddScoped<ApsEventoService>();
builder.Services.AddScoped<PersonalEventoService>();
builder.Services.AddScoped<HerramientasEquipoEventoService>();
var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseAuthorization();
app.MapControllers();
app.Run();