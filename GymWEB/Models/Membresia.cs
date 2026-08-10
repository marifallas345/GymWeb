using System;

namespace GymWEB.Models
{
    public class Membresia
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public decimal Precio { get; set; }

        public int DuracionMeses { get; set; }

        public bool Estado { get; set; }

        public DateTime Fecha_Adicion { get; set; }

        public string Creado_Por { get; set; }

        public DateTime? Fecha_Modificacion { get; set; }

        public string Modificado_Por { get; set; }
    }
}