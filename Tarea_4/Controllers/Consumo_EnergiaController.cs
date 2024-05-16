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
    public class Consumo_EnergiaController : Controller
    {
        private GREGEntities db = new GREGEntities();
        List<listaMayorDesfaceE> listaMayorDesfaceE = new List<listaMayorDesfaceE>();
        List<listaMayorYMenorConsumoE> listaMayorYMenorConsumoE = new List<listaMayorYMenorConsumoE>();

        // GET: Consumo_Energia
        public ActionResult Index()
        {
            var consumo_Energia = db.Consumo_Energia.Include(c => c.Cliente);
            return View(consumo_Energia.ToList());
        }

        // GET: Consumo_Energia/Details/5
        public ActionResult Details(int? idCliente, int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Consumo_Energia consumo_Energia = db.Consumo_Energia.Find(idCliente,id);
            if (consumo_Energia == null)
            {
                return HttpNotFound();
            }
            return View(consumo_Energia);
        }

        // GET: Consumo_Energia/Create
        public ActionResult Create()
        {
            ViewBag.IdCliente = new SelectList(db.Cliente, "IdCliente", "Nombre");
            return View();
        }

        // POST: Consumo_Energia/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdCliente,Id,Periodo,MetaAhorroEnergia,ConsumoActualEnergia")] Consumo_Energia consumo_Energia)
        {
            if (consumo_Energia.IdCliente == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (ModelState.IsValid)
            {
                db.Consumo_Energia.Add(consumo_Energia);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdCliente = new SelectList(db.Cliente, "IdCliente", "Nombre", consumo_Energia.IdCliente);
            return View(consumo_Energia);
        }

        // GET: Consumo_Energia/Edit/5
        public ActionResult Edit(int? idCliente,int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Consumo_Energia consumo_Energia = db.Consumo_Energia.Find(idCliente, id);
            if (consumo_Energia == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdCliente = new SelectList(db.Cliente, "IdCliente", "Nombre", consumo_Energia.IdCliente);
            return View(consumo_Energia);
        }

        // POST: Consumo_Energia/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdCliente,Id,Periodo,MetaAhorroEnergia,ConsumoActualEnergia")] Consumo_Energia consumo_Energia)
        {
            if (ModelState.IsValid)
            {
                db.Entry(consumo_Energia).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdCliente = new SelectList(db.Cliente, "IdCliente", "Nombre", consumo_Energia.IdCliente);
            return View(consumo_Energia);
        }

        // GET: Consumo_Energia/Delete/5
        public ActionResult Delete(int? idCliente, int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Consumo_Energia consumo_Energia = db.Consumo_Energia.Find(idCliente,id);
            if (consumo_Energia == null)
            {
                return HttpNotFound();
            }
            return View(consumo_Energia);
        }

        // POST: Consumo_Energia/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int idCliente,int id)
        {
            Consumo_Energia consumo_Energia = db.Consumo_Energia.Find(idCliente,id);
            db.Consumo_Energia.Remove(consumo_Energia);
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
        public ActionResult EstadisticasEnergia()
        {

            List<Consumo_Energia> consumosEnergia = db.Consumo_Energia.ToList();

            double PromedioEnergia = promedioEnergia(consumosEnergia);
            int valorTotalDescuento = ValorTotalDescuento(consumosEnergia);
            List<listaMayorDesfaceE> mayorDesfaceE = MayorDesfaceE(consumosEnergia);
            List<listaMayorYMenorConsumoE> mayorYMenorConsumoE = MayorYMenorConsumoE(consumosEnergia);

            EstadisticasEnergia datos = new EstadisticasEnergia
                (PromedioEnergia,
                valorTotalDescuento,
                mayorDesfaceE,
                mayorYMenorConsumoE);

            return View(datos);
        }

        public double promedioEnergia(List<Consumo_Energia> listConsumoEnergia)
        {

            int consumoTotal = listConsumoEnergia.Sum(u => u.ConsumoActualEnergia);
            int NumeroDeUsuarios = listConsumoEnergia.Count;

            double Promedio = (double)consumoTotal / NumeroDeUsuarios;
            return Promedio;
        }
        public int ValorTotalDescuento(List<Consumo_Energia> listConsumoEnergia)
        {
            int MetaAhorro = listConsumoEnergia.Sum(u => u.MetaAhorroEnergia);
            int ConsumoEnergia = listConsumoEnergia.Sum(u => u.ConsumoActualEnergia);

            int DescuentoTotal = (MetaAhorro - ConsumoEnergia) * 850;

            return DescuentoTotal;
        }
        public List<listaMayorDesfaceE> MayorDesfaceE(List<Consumo_Energia> listConsumoEnergia)
        {
            listaMayorDesfaceE.Clear();
            int Dsface = 0;
            int MayorD = 0;
            int CedulaD = 0;
            string nombre = "";
            string apellido = "";

            foreach (Consumo_Energia DesfaceE in listConsumoEnergia)
            {
                Dsface = DesfaceE.ConsumoActualEnergia - DesfaceE.MetaAhorroEnergia;
                if (Dsface > MayorD)
                {
                    MayorD = Dsface;
                    CedulaD = DesfaceE.Cliente.Cedula;
                    nombre = DesfaceE.Cliente.Nombre;
                    apellido = DesfaceE.Cliente.Apellido;
                }
            }

            listaMayorDesfaceE data = new listaMayorDesfaceE(CedulaD, nombre, apellido, MayorD);
            listaMayorDesfaceE.Add(data);


            return listaMayorDesfaceE;
        }
        public List<listaMayorYMenorConsumoE> MayorYMenorConsumoE(List<Consumo_Energia> consumosEnergia)
        {
            listaMayorYMenorConsumoE.Clear();
            Dictionary<int, int> consumoEnergiaPorEstrato = new Dictionary<int, int>();

            foreach (Consumo_Energia consumoE in consumosEnergia)
            {
                if (consumoEnergiaPorEstrato.ContainsKey(consumoE.Cliente.Estrato))
                {
                    consumoEnergiaPorEstrato[consumoE.Cliente.Estrato] += consumoE.ConsumoActualEnergia;
                }
                else
                {
                    consumoEnergiaPorEstrato[consumoE.Cliente.Estrato] = consumoE.ConsumoActualEnergia;
                }
            }

            int estratoMayorConsumo = -1;
            int estratoMenorConsumo = -1;
            int maxConsumo = int.MinValue;
            int minConsumo = int.MaxValue;

            foreach (int estrato in consumoEnergiaPorEstrato.Keys)
            {
                if (consumoEnergiaPorEstrato[estrato] > maxConsumo)
                {
                    maxConsumo = consumoEnergiaPorEstrato[estrato];
                    estratoMayorConsumo = estrato;
                }
                if (consumoEnergiaPorEstrato[estrato] < minConsumo)
                {
                    minConsumo = consumoEnergiaPorEstrato[estrato];
                    estratoMenorConsumo = estrato;
                }
            }
            listaMayorYMenorConsumoE data = new listaMayorYMenorConsumoE(estratoMayorConsumo, estratoMenorConsumo);
            listaMayorYMenorConsumoE.Add(data);

            return listaMayorYMenorConsumoE;
        }
       
    }
}
