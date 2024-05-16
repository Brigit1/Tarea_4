using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tarea_3.Models;

namespace Tarea_4.Models
{
    public class EstadisticasEnergia
    {
        public EstadisticasEnergia(
        double promedioEnergia,
        int valorTotalDescuento,
        List<listaMayorDesfaceE> mayorDesfaceE,
        List<listaMayorYMenorConsumoE> mayorYMenorConsumoE
    )
        {
            this.PromedioEnergia = promedioEnergia;
            this.ValorTotalDescuento = valorTotalDescuento;
            this.MayorDesfaceE = mayorDesfaceE;
            this.MayorYMenorConsumoE = mayorYMenorConsumoE;
        }
        public double PromedioEnergia { get; set; }
        public int ValorTotalDescuento { get; set; }
        public List<listaMayorDesfaceE> MayorDesfaceE { get; set; }
        public List<listaMayorYMenorConsumoE> MayorYMenorConsumoE { get; set; }
    }
}