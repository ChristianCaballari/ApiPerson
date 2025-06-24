using System.Text.Json.Serialization;

namespace ApiPerson.Entidades
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum CampoFiltro
    {
        nombre,
        apellido,
        email
    }

}
