using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tarea_4.Models
{
    public class EstadisticasGenerales
    {
       
        public EstadisticasGenerales(
 
        int totalPagarEmpresa
    )
        {
         this.TotalPagarEmpresa = totalPagarEmpresa;
        }
        public int TotalPagarEmpresa { get; set; }
    }
}