using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Contoso.MVC.Models
{
    public class DatosAbogadoDto
    {
        public string RetornoId { get; set; }
        public int Codigo { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
       public virtual List<Quinquenio> Quinquenio { get; set; }
    }

    public class Quinquenio
    {
        public int Codigo { get; set; }
        public DateTime? FechaInicio { get;set;}

        public DateTime? FechaFin { get; set; }

        public int Vigente { get; set; }
        

    }
}
