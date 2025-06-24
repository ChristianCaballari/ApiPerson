using System.ComponentModel.DataAnnotations;

namespace ApiPerson.DTOs
{
    public class PersonaCreacionDTO
    {
        [Required(ErrorMessage ="El campo {0} es requerido.")]
        public string Nombre { get; set; }
        
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string Apellido { get; set; }
        
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Fecha Nacimiento")]
        public DateTime FechaNacimiento { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [EmailAddress(ErrorMessage ="El email debe ser válido")]
        public string Email { get; set; }
        public string? Telefono { get; set; }
        public string? Direccion { get; set; }
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