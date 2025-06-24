using ApiPerson.DTOs;
using ApiPerson.Entities;
using AutoMapper;

namespace ApiPerson.Utilidades
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles() 
        {
            CreateMap<PersonaCreacionDTO, PersonaDTO>();
            CreateMap<Persona, PersonaDTO>();
            CreateMap<PersonaCreacionDTO, Persona>();
        }
    }
}
