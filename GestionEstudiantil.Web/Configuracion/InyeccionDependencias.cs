using GestionEstudiantil.Infraestructura.Datos;
using Microsoft.EntityFrameworkCore;
using GestionEstudiantil.Aplicacion.Servicios;


namespace GestionEstudiantil.Web.Configuracion;

public static class InyeccionDependencias
{
    public static IServiceCollection AgregarServiciosInfraestructura(this IServiceCollection servicios, IConfiguration configuracion)
    {
        servicios.AddDbContext<BaseDeDatosContexto>(opciones =>
            opciones.UseSqlite(configuracion.GetConnectionString("BaseDeDatosSQLite")));

        return servicios;
    }
}
