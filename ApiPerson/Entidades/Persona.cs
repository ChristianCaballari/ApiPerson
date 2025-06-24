using System.ComponentModel.DataAnnotations;

namespace ApiPerson.Entities
{
    public class Persona
    {
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public DateTime FechaRegistro  { get; set; }
    }
}


/*● Id(int, clave primaria)
● Nombre(string, requerido)
● Apellido(string, requerido)
● FechaNacimiento(DateTime, requerido)
● Email(string, requerido, formato válido)
● Telefono(string, opcional)
● Direccion(string, opcional)
● FechaRegistro(DateTime, automático)
*/