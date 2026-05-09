using Microsoft.EntityFrameworkCore;
using ProyecServicio_comunitario.Models;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// Configurar PostgreSQL
builder.Services.AddDbContext<AngelDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.OpenApiInfo 
    { 
        Title = "AngelDB API", 
        Version = "v1" 
    });
});
var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();




app.UseAuthorization();
app.MapControllers();
app.Run();