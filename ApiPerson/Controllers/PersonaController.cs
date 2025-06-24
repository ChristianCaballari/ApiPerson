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
        [HttpGet]
        public async Task<ActionResult<List<Persona>>> Get()
        {
            var personas = await _context.Personas.ToListAsync();
            return Ok(personas);
        }

        /// <summary>
        /// Recupera una persona por su ID.
        /// </summary>
        /// <param name="id">ID de la persona a recuperar.</param>
        /// <returns>La persona con el ID especificado.</returns>

        [HttpGet("{id:int}", Name = "obtenerPorId")]
        public async Task<ActionResult<PersonaDTO>> Get(int id)
        {
            var persona = await _context.Personas.FirstOrDefaultAsync(x => x.Id == id);

            if (persona is null)
            {
                return NotFound();
            }
            return _mapper.Map<PersonaDTO>(persona);
        }


        /// <summary>
        /// Registro de personas.
        /// </summary>
        /// <param name="personaCreacionDTO">DTO con los parametros de la persona a insertar.</param>
        /// <returns>La persona recien ingresada</returns>
        [HttpPost(Name = "crearPersona")]
        public async Task<ActionResult> Post([FromBody] PersonaCreacionDTO personaCreacionDTO)
        {

            var num = 5;
          var persona = _mapper.Map<Persona>(personaCreacionDTO);

            _context.Add(persona);
            await _context.SaveChangesAsync();
            var personaDTO = _mapper.Map<PersonaDTO>(persona);
            return CreatedAtRoute("obtenerPorId", new { id = persona.Id }, personaDTO);
        }

        /// <summary>
        /// Actualizar personas por su Id.
        /// </summary>
        /// <param name="personaCreacionDTO">DTO con los parametros de la persona a actualizar</param>
        /// <param name="id">Id de la persona a actualizar</param>
        /// <returns>La persona recien ingresada</returns>
        [HttpPut("{id:int}", Name = "ActualizarPersona")]
        public async Task<ActionResult> Put(PersonaCreacionDTO personaCreacionDTO, int id)
        {
            var existe = await _context.Personas.AnyAsync(x => x.Id == id);

            if (!existe) 
            {
                return NotFound();
            }
            personaCreacionDTO.FechaNacimiento = DateTime.SpecifyKind(personaCreacionDTO.FechaNacimiento, DateTimeKind.Utc);


            var persona = _mapper.Map<Persona>(personaCreacionDTO);
            persona.Id = id;
            _context.Update(persona);
            await _context.SaveChangesAsync();
            return NoContent();
        }


        /// <summary>
        /// Eliminar una persona por su ID.
        /// </summary>
        /// <param name="id">ID de la persona a eliminar.</param>
        [HttpDelete("{id:int}", Name = "eliminarPorId")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await _context.Personas.AnyAsync(x => x.Id == id);

            if (!existe) 
            {
                return NotFound();
            }
            _context.Remove(new Persona() { Id = id });
            await _context.SaveChangesAsync();
            return Ok();
        }

        /// <summary>
        /// Busca personas por filtro (nombre,apellido,email).
        /// </summary>
        /// <param name="valor">Valor a buscar.</param>
        /// <param name="filtro">Filtro por el que se va a buscar.</param>
        /// <returns>Recupera Lista de personas.</returns>
        [HttpGet("buscar")]
        public async Task<IActionResult> Buscar([FromQuery] CampoFiltro? filtro, [FromQuery] string? valor)
        {
           
            if (string.IsNullOrWhiteSpace(valor) || filtro ==  null)
                return BadRequest(new { mensaje = "Debe especificar un valor para buscar y especificar el fitro" });

            var query = _context.Personas.AsQueryable();

            switch (filtro)
            {
                case CampoFiltro.nombre:
                    query = query.Where(p => p.Nombre.Contains(valor));
                    break;
                case CampoFiltro.apellido:
                    query = query.Where(p => p.Apellido.Contains(valor));
                    break;
                case CampoFiltro.email:
                    query = query.Where(p => p.Email.Contains(valor));
                    break;
            }

            var resultado = await query.ToListAsync();
            return Ok(resultado);
        }
    }
}
