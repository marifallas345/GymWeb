using Newtonsoft.Json;

namespace GymWEB.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        [JsonProperty("usuario")]
        public string UsuarioLogin { get; set; }

        public string Rol { get; set; }
    }
}