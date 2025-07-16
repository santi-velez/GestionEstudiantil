using GestionEstudiantil.Web.Configuracion;
using GestionEstudiantil.Infraestructura.Datos;
using GestionEstudiantil.Dominio.Entidades;
using GestionEstudiantil.Aplicacion.Servicios; // Asegúrate de tener este using
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Agrega los servicios MVC
builder.Services.AddControllersWithViews();

// Agrega servicios de infraestructura (como el DbContext)
builder.Services.AgregarServiciosInfraestructura(builder.Configuration);

// Agrega InscripcionService para que pueda ser inyectado en los controladores
builder.Services.AddScoped<InscripcionService>();

var app = builder.Build();

// Poblar base de datos con relación inicial si está vacía
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<BaseDeDatosContexto>();

    if (!context.EstudiantesAsignaturas.Any())
    {
        var estudiante = context.Estudiantes.FirstOrDefault();
        var asignatura = context.Asignaturas.FirstOrDefault();

        if (estudiante != null && asignatura != null)
        {
            context.EstudiantesAsignaturas.Add(new EstudianteAsignatura
            {
                EstudianteId = estudiante.Id,
                AsignaturaId = asignatura.Id
            });

            context.SaveChanges();
        }
    }
}

// Configuración de errores y HTTPS
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // Asegúrate de tener esto si vas a servir CSS, JS, etc.

app.UseRouting();
app.UseAuthorization();

// Rutas por defecto
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
