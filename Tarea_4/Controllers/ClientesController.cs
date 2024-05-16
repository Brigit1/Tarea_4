using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Tarea_4;
using Tarea_4.Models;

namespace Tarea_4.Controllers
{
    public class ClientesController : Controller
    {
        private GREGEntities db = new GREGEntities();
        //private static CalculosInformacion calculosInformacion = new CalculosInformacion();
        // GET: Clientes
        public ActionResult Index()
        {
            return View(db.Cliente.ToList());
        }

        // GET: Clientes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = db.Cliente.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        // GET: Clientes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Clientes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdCliente,Cedula,Nombre,Apellido,Celular,Email,Estrato")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                db.Cliente.Add(cliente);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cliente);
        }

        // GET: Clientes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = db.Cliente.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        // POST: Clientes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdCliente,Cedula,Nombre,Apellido,Celular,Email,Estrato")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cliente).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cliente);
        }

        // GET: Clientes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = db.Cliente.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cliente cliente = db.Cliente.Find(id);
            db.Cliente.Remove(cliente);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
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
        public ActionResult estadisticas()
        {

            List<Consumo_Energia> consumosEnergia = db.Consumo_Energia.ToList();
            List<Consumo_Agua> ConsumoAgua = db.Consumo_Agua.ToList();

            int totalPagarEmpresa = TotalPagarEmpresa(consumosEnergia, ConsumoAgua);

            EstadisticasGenerales datos = new EstadisticasGenerales(totalPagarEmpresa);

            return View(datos);
        }


        public int TotalPagarEmpresa(List<Consumo_Energia> listConsumoEnergia, List<Consumo_Agua> listaConsumoAgua)
        {

            int ConsumoTotalEnergia = listConsumoEnergia.Sum(u => u.ConsumoActualEnergia);
            int MetaTotalEnergia = listConsumoEnergia.Sum(u => u.MetaAhorroEnergia);

            int ConsumoTotalAgua = listaConsumoAgua.Sum(u => u.ConsumoActualAgua);
            int PromedioTotalAgua = listaConsumoAgua.Sum(u => u.PromedioConsumoAgua);

            int valorParcialEnergia = ConsumoTotalEnergia * 850;
            int ValorIncentivo = (MetaTotalEnergia - ConsumoTotalEnergia) * 850;
            int ValorTotalEnergia = valorParcialEnergia - ValorIncentivo;

            int ConsumoAgua = PromedioTotalAgua * 4600;
            int ExcesoAgua = (ConsumoTotalAgua - PromedioTotalAgua) * 9200;
            int ValorTotalAgua = ConsumoAgua + ExcesoAgua;

            int PagoTotal = ValorTotalEnergia + ValorTotalAgua;

            return PagoTotal;
        }

    }
}

