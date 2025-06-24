using ApiPerson;
using ApiPerson.Entidades;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
var origenesPermitidos = builder.Configuration.GetValue<string>("origenesPermitidos")!;

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Configurar el Application DB Context con PostgreSQL
builder.Services.AddDbContext<AppDbContext>(opciones =>
  opciones.UseNpgsql(builder.Configuration.GetConnectionString("defaultConnection")));

// Configurar el AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

// Configurar nuestra cors

builder.Services.AddCors(opciones =>
{
    opciones.AddDefaultPolicy(configuracion =>
    {
        configuracion.WithOrigins(origenesPermitidos).AllowAnyHeader().AllowAnyMethod();
    });
});


builder.Services.AddEndpointsApiExplorer();//permitir que swagger permita explorar los endpoints y listarlos
builder.Services.AddSwaggerGen(c => 
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Personas API",
        Description = "Este es un WEB API para trabajar con datos de personas",
        Contact = new OpenApiContact 
        {
          Email = "christiamccm17@gmail.com",
          Name = "Christian Jesus Caballari Calderón",
          Url = new Uri("https://www.linkedin.com/in/christiancaballari/")
        },
        License = new OpenApiLicense
        {
            Name = "MIT",
            Url = new Uri("https://opensource.org/licence/mit")
        }
    });

    // Mostrar descripcion de los endpoints en swagger
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });


// Configurar nuestros services

var app = builder.Build();


// Middleware
app.UseExceptionHandler(exceptionHandlerApp =>
{
    exceptionHandlerApp.Run(async context =>
    {
        var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
        var excepcion = exceptionHandlerFeature?.Error!;

        // Crear entidad de error
        var error = new Error
        {
            Fecha = DateTime.UtcNow,
            MensajeDeError = excepcion.Message,
            StackTrace = excepcion.StackTrace
        };

        // Obtener el DbContext directamente desde el contenedor
        var dbContext = context.RequestServices.GetRequiredService<AppDbContext>();
        dbContext.Errores.Add(error);
        await dbContext.SaveChangesAsync();

        // Devolver una respuesta personalizada
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/json";

        var response = new
        {
            tipo = "error",
            mensaje = "Ha ocurrido un error inesperado.",
            estatus = 500
        };

        await context.Response.WriteAsJsonAsync(response);
    });
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
