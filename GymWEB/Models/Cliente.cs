using Newtonsoft.Json;
using System;

namespace GymWEB.Models
{
    public class Cliente
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public string Cedula { get; set; }

        public string Telefono { get; set; }

        public string Email { get; set; }

        public DateTime FechaNacimiento { get; set; }

        public string Sexo { get; set; }

        public bool Estado { get; set; }

        [JsonProperty("fecha_Adicion")]
        public DateTime Fecha_Adicion { get; set; }

        [JsonProperty("creado_Por")]
        public string Creado_Por { get; set; }

        [JsonProperty("fecha_Modificacion")]
        public DateTime? Fecha_Modificacion { get; set; }

        [JsonProperty("modificado_Por")]
        public string Modificado_Por { get; set; }
    }
}