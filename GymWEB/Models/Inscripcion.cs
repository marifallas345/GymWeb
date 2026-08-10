using System;

namespace GymWEB.Models
{
    public class Inscripcion
    {
        public int Id { get; set; }

        public int ClienteId { get; set; }

        public int MembresiaId { get; set; }

        public DateTime FechaInicio { get; set; }

        public DateTime FechaVencimiento { get; set; }

        public bool Estado { get; set; }

        public DateTime Fecha_Adicion { get; set; }

        public string Creado_Por { get; set; }

        public DateTime? Fecha_Modificacion { get; set; }

        public string Modificado_Por { get; set; }
    }
}