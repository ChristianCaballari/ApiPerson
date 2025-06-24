using ApiPerson.DTOs;
using ApiPerson.Entidades;
using ApiPerson.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiPerson.Controllers
{
    [ApiController]
    [Route("api/personas")]
    public class PersonaController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public PersonaController(AppDbContext context,IMapper mapper) {
            this._context = context;
            this._mapper = mapper;
        }

        /// <summary>
        /// Recupera el listado de personas.
        /// </summary>
        [HttpGet(Name = "obtenerTodasLasPersonas")]
        [ProducesResponseType(typeof(List<PersonaDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<PersonaDTO>>> Get()
        {
            try
            {
                var personas = await _context.Personas.ToListAsync();
                var personasDTO = _mapper.Map<List<PersonaDTO>>(personas);

                return Ok(personasDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error inesperado al recuperar las personas." });
            }
        }

        /// <summary>
        /// Recupera una persona por su ID.
        /// </summary>
        /// <param name="id">ID de la persona a recuperar.</param>
        /// <returns>La persona con el ID especificado.</returns>

        [HttpGet("{id:int}", Name = "obtenerPorId")]
        [ProducesResponseType(typeof(PersonaDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PersonaDTO>> Get(int id)
        {
            try
            {
                var persona = await _context.Personas.FirstOrDefaultAsync(x => x.Id == id);

                if (persona is null)
                    return NotFound(new { mensaje = $"No se encontró una persona con ID {id}." });

                var personaDTO = _mapper.Map<PersonaDTO>(persona);
                return Ok(personaDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error inesperado al obtener la persona." });
            }
        }

        /// <summary>
        /// Registro de personas.
        /// </summary>
        /// <param name="personaCreacionDTO">DTO con los parametros de la persona a insertar.</param>
        /// <returns>La persona recien ingresada</returns>
        [HttpPost(Name = "crearPersona")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PersonaDTO>> Post([FromBody] PersonaCreacionDTO personaCreacionDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var persona = _mapper.Map<Persona>(personaCreacionDTO);
                persona.FechaRegistro = DateTime.UtcNow;

                _context.Personas.Add(persona);
                await _context.SaveChangesAsync();

                var personaDTO = _mapper.Map<PersonaDTO>(persona);
                return CreatedAtRoute("obtenerPorId", new { id = persona.Id }, personaDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Ocurrió un error inesperado al crear la persona." });
            }
        }


        /// <summary>
        /// Actualizar personas por su Id.
        /// </summary>
        /// <param name="personaCreacionDTO">DTO con los parametros de la persona a actualizar</param>
        /// <param name="id">Id de la persona a actualizar</param>
        /// <returns>La persona recien ingresada</returns>
        [HttpPut("{id:int}", Name = "ActualizarPersona")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Put([FromBody] PersonaCreacionDTO personaCreacionDTO, int id)
        {
            if (personaCreacionDTO == null || !ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var existe = await _context.Personas.AnyAsync(x => x.Id == id);
                if (!existe)
                    return NotFound();

                personaCreacionDTO.FechaNacimiento = DateTime.SpecifyKind(
                    personaCreacionDTO.FechaNacimiento, DateTimeKind.Utc);

                var persona = _mapper.Map<Persona>(personaCreacionDTO);
                persona.Id = id;

                _context.Update(persona);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error inesperado al actualizar la persona." });
            }
        }

        /// <summary>
        /// Eliminar una persona por su ID.
        /// </summary>
        /// <param name="id">ID de la persona a eliminar.</param>
        [HttpDelete("{id:int}", Name = "eliminarPorId")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var persona = await _context.Personas.FindAsync(id);
                if (persona == null)
                {
                    return NotFound(new { mensaje = $"No se encontró una persona con ID {id} para eliminar." });
                }

                _context.Personas.Remove(persona);
                await _context.SaveChangesAsync();

                return NoContent(); // 204
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error inesperado al intentar eliminar la persona." });
            }
        }


        /// <summary>
        /// Busca personas por filtro (nombre,apellido,email).
        /// </summary>
        /// <param name="valor">Valor a buscar.</param>
        /// <param name="filtro">Filtro por el que se va a buscar.</param>
        /// <returns>Recupera Lista de personas.</returns>
        [HttpGet("buscar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Buscar([FromQuery] CampoFiltro? filtro, [FromQuery] string? valor)
        {
            if (string.IsNullOrWhiteSpace(valor) || filtro == null)
                return BadRequest(new { mensaje = "Debe especificar un valor para buscar y un filtro válido (nombre, apellido o email)." });

            try
            {
                var query = _context.Personas.AsQueryable();

                string valorNormalizado = valor.Trim().ToLower();

                switch (filtro)
                {
                    case CampoFiltro.nombre:
                        query = query.Where(p => p.Nombre.ToLower().Contains(valorNormalizado));
                        break;
                    case CampoFiltro.apellido:
                        query = query.Where(p => p.Apellido.ToLower().Contains(valorNormalizado));
                        break;
                    case CampoFiltro.email:
                        query = query.Where(p => p.Email.ToLower().Contains(valorNormalizado));
                        break;
                    default:
                        return BadRequest(new { mensaje = "Filtro no reconocido. Use: nombre, apellido o email." });
                }

                var resultado = await query.ToListAsync();

                if (resultado.Count == 0)
                    return Ok(new { mensaje = "No se encontraron personas que coincidan con la búsqueda." });

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error inesperado al realizar la búsqueda." });
            }
        }
    }
}
