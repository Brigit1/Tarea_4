using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tarea_3.Models;
using Tarea_4.Models;

namespace Tarea_4.Controllers
{
    public class HomeController : Controller
    {
        private GREGEntities db = new GREGEntities();
        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        public ActionResult Factura(int? id, int mes)
        {
            Cliente cliente = db.Cliente.Find(id);
            try
            {
                db.Entry(cliente).Collection(c => c.Consumo_Agua).Load();
                db.Entry(cliente).Collection(c => c.Consumo_Energia).Load();

                int facturaAgua = FacturasAgua(cliente.Consumo_Agua.ToList(), mes);
                int facturaEnergia = FacturasEnergia(cliente.Consumo_Energia.ToList(), mes);
                int Total = facturaAgua + facturaEnergia;
                CalculosInformacion calculosInformacion = new CalculosInformacion
                {
                    Nombre = cliente.Nombre,
                    Apellido = cliente.Apellido,
                    Cedula = cliente.Cedula,
                    Energia = facturaEnergia,
                    Agua = facturaAgua,
                    Total = Total
                };
                return View(calculosInformacion);
            }
            catch (Exception ex)
            {
                // Maneja la excepción mostrando un mensaje de error o registrando información adicional
                Console.WriteLine("Error al cargar los datos de Consumo_Agua: " + ex.Message);
                return View();
            }
        }

        public int FacturasAgua(List<Consumo_Agua> consumo_Aguas, int mes)
        {

            var consumosDelMes = consumo_Aguas.Where(c => c.Periodo == mes);

            // Calcular el promedio de consumo de agua para el mes seleccionado
            if (consumosDelMes.Any())
            {
                foreach (var consumo in consumosDelMes)
                {
                    int promedioAgua = consumo.PromedioConsumoAgua;
                    int consumoAgua = consumo.ConsumoActualAgua;

                    int valorPromedio = promedioAgua * 4600;
                    int valorExceso = (consumoAgua - promedioAgua) * (2 * 4600);
                    int valorDeAgua = valorPromedio + valorExceso;

                    return valorDeAgua;

                }
            }
            return 0; // No hay datos de consumo para el mes seleccionado
        }
        public int FacturasEnergia(List<Consumo_Energia> consumo_energia, int mes)
        {
            var consumosDelMes = consumo_energia.Where(c => c.Periodo == mes);

            // Calcular el promedio de consumo de agua para el mes seleccionado
            if (consumosDelMes.Any())
            {
                foreach (var consumo in consumosDelMes)
                {
                    int metaAhorroEnergia = consumo.MetaAhorroEnergia;
                    int consumoEnergia = consumo.ConsumoActualEnergia;

                    int valorParcial = consumoEnergia * 850;
                    int valorInsentivo = (metaAhorroEnergia - consumoEnergia) * 850;
                    int valorEnergia = valorParcial - valorInsentivo;

                    return valorEnergia;

                }
            }
            return 0;
        }
    }
}