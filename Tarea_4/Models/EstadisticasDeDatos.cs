using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tarea_4.Controllers;

namespace Tarea_3.Models
{
    public class EstratoPorcentaje
    {
        int estrato;
        double porcentaje;

        public EstratoPorcentaje(int estrato, double porcentaje)
        {
            this.Estrato = estrato;
            this.Porcentaje = porcentaje;
        }

        public int Estrato { get => estrato; set => estrato = value; }
        public double Porcentaje { get => porcentaje; set => porcentaje = value; }

    }
    public class listaConsumoAguaMayorPromedio
    {
        int cedula;
        string nombre;
        string apellido;
        int excesoA;

        public listaConsumoAguaMayorPromedio(int cedula, string nombre, string apellido, int excesoA)
        {
            this.cedula = cedula;
            this.nombre = nombre;
            this.apellido = apellido;
            this.ExcesoA = excesoA;
        }
        public int Cedula { get => cedula; set => cedula = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public string Apellido { get => apellido; set => apellido = value; }
        public int ExcesoA { get => excesoA; set => excesoA = value; }


    }
    public class listaMayorDesfaceE
    {
        int cedula;
        string nombre;
        string apellido;
        int mayorD;

        public listaMayorDesfaceE(int cedula, string nombre, string apellido, int mayorD)
        {
            this.cedula = cedula;
            this.nombre = nombre;
            this.apellido = apellido;
            this.MayorD = mayorD;
        }
        public int Cedula { get => cedula; set => cedula = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public string Apellido { get => apellido; set => apellido = value; }
        public int MayorD { get => mayorD; set => mayorD = value; }


    }
    public class listaMayorYMenorConsumoE
    {
        int estratoMayorConsumo;
        int estratoMenorConsumo;

        public listaMayorYMenorConsumoE(int estratoMayorConsumo, int estratoMenorConsumo)
        {
            this.EstratoMayorConsumo = estratoMayorConsumo;
            this.EstratoMenorConsumo = estratoMenorConsumo;
        }

        public int EstratoMayorConsumo { get => estratoMayorConsumo; set => estratoMayorConsumo = value; }
        public int EstratoMenorConsumo { get => estratoMenorConsumo; set => estratoMenorConsumo = value; }

    }
    public class EstadisticasDeDatos
    {
        List<EstratoPorcentaje> listaEstratosPorcentaje = new List<EstratoPorcentaje>();
        List<listaConsumoAguaMayorPromedio> listaConsumoAguaMayorPromedio = new List<listaConsumoAguaMayorPromedio>();
        List<listaMayorDesfaceE> listaMayorDesfaceE = new List<listaMayorDesfaceE>();
        List<listaMayorYMenorConsumoE> listaMayorYMenorConsumoE = new List<listaMayorYMenorConsumoE>();
      
    }
}