using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tarea_3.Models;

namespace Tarea_4.Models
{
    public class EstadisticasAgua
    {   public EstadisticasAgua(
       int totalDeExcesosDeAgua,
       List<EstratoPorcentaje> excesoDeAguaPorEstrato,
       List<listaConsumoAguaMayorPromedio> consumoAguaMayorPromedio,
       int estratoMayorAhorroAgua)
        {
            this.TotalDeExcesosDeAgua = totalDeExcesosDeAgua;
            this.ExcesoDeAguaPorEstrato = excesoDeAguaPorEstrato;
            this.ConsumoAguaMayorPromedio = consumoAguaMayorPromedio;
            this.EstratoMayorAhorroAgua = estratoMayorAhorroAgua;

        } 

        public int TotalDeExcesosDeAgua { get; set; }
        public List<EstratoPorcentaje> ExcesoDeAguaPorEstrato { get; set; }
        public List<listaConsumoAguaMayorPromedio> ConsumoAguaMayorPromedio { get; set; }
        public int EstratoMayorAhorroAgua { get; set; }
    }
}