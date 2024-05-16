using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Tarea_3.Models;
using Tarea_4;
using Tarea_4.Models;

namespace Tarea_4.Controllers
{
    public class Consumo_AguaController : Controller
    {
        private GREGEntities db = new GREGEntities();
        List<EstratoPorcentaje> listaEstratosPorcentaje = new List<EstratoPorcentaje>();
        List<listaConsumoAguaMayorPromedio> listaConsumoAguaMayorPromedio = new List<listaConsumoAguaMayorPromedio>();
 

        // GET: Consumo_Agua
        public ActionResult Index()
        {
            var consumo_Agua = db.Consumo_Agua.Include(c => c.Cliente);
            return View(consumo_Agua.ToList());
        }

        // GET: Consumo_Agua/Details/5
        public ActionResult Details(int? idCliente,int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Consumo_Agua consumo_Agua = db.Consumo_Agua.Find(idCliente,id);
            if (consumo_Agua == null)
            {
                return HttpNotFound();
            }
            return View(consumo_Agua);
        }

        // GET: Consumo_Agua/Create
        public ActionResult Create()
        {
            ViewBag.IdCliente = new SelectList(db.Cliente, "IdCliente", "Nombre");
            return View();
        }

        // POST: Consumo_Agua/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdCliente,Id,ConsumoActualAgua,Periodo,PromedioConsumoAgua")] Consumo_Agua consumo_Agua)
        {
            if (consumo_Agua.IdCliente==0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (ModelState.IsValid)
            {
                db.Consumo_Agua.Add(consumo_Agua);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdCliente = new SelectList(db.Cliente, "IdCliente", "Nombre", consumo_Agua.IdCliente);
            return View(consumo_Agua);
        }

        // GET: Consumo_Agua/Edit/5
        public ActionResult Edit(int? idCliente, int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Consumo_Agua consumo_Agua = db.Consumo_Agua.Find(idCliente,id);
            if (consumo_Agua == null)
            {
                return HttpNotFound();
            }
            return View(consumo_Agua);
        }

        // POST: Consumo_Agua/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdCliente,Id,ConsumoActualAgua,Periodo,PromedioConsumoAgua")] Consumo_Agua consumo_Agua)
        {
            if (ModelState.IsValid)
            {
                db.Entry(consumo_Agua).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdCliente = new SelectList(db.Cliente, "IdCliente", "Nombre", consumo_Agua.IdCliente);
            return View(consumo_Agua);
        }

        // GET: Consumo_Agua/Delete/5
        public ActionResult Delete(int? idCliente, int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Consumo_Agua consumo_Agua = db.Consumo_Agua.Find(idCliente,id);
            if (consumo_Agua == null)
            {
                return HttpNotFound();
            }
            return View(consumo_Agua);
        }

        // POST: Consumo_Agua/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int idCliente,int id)
        {
            Consumo_Agua consumo_Agua = db.Consumo_Agua.Find(idCliente,id);
            db.Consumo_Agua.Remove(consumo_Agua);
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
        public ActionResult EstadisticasAgua()
        {

            List<Consumo_Agua> ConsumoAgua = db.Consumo_Agua.ToList();

            int TotalDeExcesosDeAgua = totalDeExcesosDeAgua(ConsumoAgua);
            List<EstratoPorcentaje> excesoDeaguaPorEstrato = ExcesoDeaguaPorEstrato(ConsumoAgua);
            List<listaConsumoAguaMayorPromedio> consumoAguaMayorPromedio = ConsumoAguaMayorPromedio(ConsumoAgua);
            int estratoMayorAhorroAgua = EstratoMayorAhorroAgua(ConsumoAgua);

            EstadisticasAgua datos = new EstadisticasAgua
                (TotalDeExcesosDeAgua,
                excesoDeaguaPorEstrato,
                consumoAguaMayorPromedio,
                estratoMayorAhorroAgua);

            return View(datos);
        }

      
        public int totalDeExcesosDeAgua(List<Consumo_Agua> listaConsumoAgua)
        {
            int PromedioAgua = listaConsumoAgua.Sum(u => u.PromedioConsumoAgua);
            int ConsumoAgua = listaConsumoAgua.Sum(u => u.ConsumoActualAgua);

            int ExcesoTotal = ConsumoAgua - PromedioAgua;
            return ExcesoTotal;
        }
        public List<EstratoPorcentaje> ExcesoDeaguaPorEstrato(List<Consumo_Agua> listaConsumoAgua)
        {
            listaEstratosPorcentaje.Clear();
            Dictionary<int, int> consumoExcesivo = new Dictionary<int, int>();
            Dictionary<int, int> totalEstrato = new Dictionary<int, int>();
            int ExcesoT = 0;
            foreach (Consumo_Agua Exceso in listaConsumoAgua)
            {
                ExcesoT = Exceso.ConsumoActualAgua - Exceso.PromedioConsumoAgua;
                if (ExcesoT > 0)
                {
                    if (consumoExcesivo.ContainsKey(Exceso.Cliente.Estrato))
                    {
                        consumoExcesivo[Exceso.Cliente.Estrato]++;
                    }
                    else
                    {
                        consumoExcesivo[Exceso.Cliente.Estrato] = 1;
                    }
                }
                if (totalEstrato.ContainsKey(Exceso.Cliente.Estrato))
                {
                    totalEstrato[Exceso.Cliente.Estrato]++;
                }
                else
                {
                    totalEstrato[Exceso.Cliente.Estrato] = 1;
                }
            }

            foreach (int estrato in totalEstrato.Keys)
            {

                int clientesExcesivos = consumoExcesivo.ContainsKey(estrato) ? consumoExcesivo[estrato] : 0;
                double porcentaje = (double)clientesExcesivos / totalEstrato[estrato] * 100;
                EstratoPorcentaje data = new EstratoPorcentaje(estrato, porcentaje);
                listaEstratosPorcentaje.Add(data);

            }
            return listaEstratosPorcentaje;
        }
        public List<listaConsumoAguaMayorPromedio> ConsumoAguaMayorPromedio(List<Consumo_Agua> listaConsumoAgua)
        {
            listaConsumoAguaMayorPromedio.Clear();
            int ExcesoA = 0;


            foreach (Consumo_Agua ExcesoAgua in listaConsumoAgua)
            {
                ExcesoA = ExcesoAgua.ConsumoActualAgua - ExcesoAgua.PromedioConsumoAgua;
                if (ExcesoA > 0)
                {

                    listaConsumoAguaMayorPromedio data = new listaConsumoAguaMayorPromedio(ExcesoAgua.Cliente.Cedula, ExcesoAgua.Cliente.Nombre, ExcesoAgua.Cliente.Apellido, ExcesoA);
                    listaConsumoAguaMayorPromedio.Add(data);

                }
            }
            return listaConsumoAguaMayorPromedio;
        }

        public int EstratoMayorAhorroAgua(List<Consumo_Agua> listaConsumoAgua)
        {
            Dictionary<int, int> ahorroEstrato = new Dictionary<int, int>();

            foreach (Consumo_Agua estratos in listaConsumoAgua)
            {
                int ahorro = estratos.PromedioConsumoAgua - estratos.ConsumoActualAgua;
                if (ahorroEstrato.ContainsKey(estratos.Cliente.Estrato))
                {
                    ahorroEstrato[estratos.Cliente.Estrato] += ahorro;
                }
                else
                {
                    ahorroEstrato[estratos.Cliente.Estrato] = ahorro;
                }
            }

            int estratoMayorAhorro = -1;
            int maxAhorro = int.MinValue;

            foreach (int estrato in ahorroEstrato.Keys)
            {
                if (ahorroEstrato[estrato] > maxAhorro)
                {
                    maxAhorro = ahorroEstrato[estrato];
                    estratoMayorAhorro = estrato;
                }
            }
            return estratoMayorAhorro;
        }
      
    }
}
