using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GlabsHelps.Data;
using GlabsHelps.Models;

namespace GlabsHelps.Controllers
{
    public class EquiposController : Controller
    {
        private GlabsHelpsContext db = new GlabsHelpsContext();

        // GET: Equipos
        public ActionResult Index(string busqueda)
        {

            var equipos = from s in db.Equipos select s;

            if (!string.IsNullOrEmpty(busqueda))
            {
                equipos = equipos.Where(s => s.IdCliente.ToString().Contains(busqueda) || s.Descripcion.Contains(busqueda) || s.Responsable.Contains(busqueda)
                || s.DireccionAnyDesk.Contains(busqueda) || s.DireccionTeamViewer.Contains(busqueda) || s.IpEquipo.Contains(busqueda)
                || s.IpPublica.Contains(busqueda) || s.IpLocal.Contains(busqueda) || s.TipoEquipo.Contains(busqueda) || s.UsuarioEquipo.Contains(busqueda)
                || s.ClaveEquipo.Contains(busqueda));
            }
            return View(equipos.ToList());
        }

        // GET: Equipos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Equipos equipos = db.Equipos.Find(id);
            if (equipos == null)
            {
                return HttpNotFound();
            }
            return View(equipos);
        }

        // GET: Equipos/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdEquipo,IdCliente,Descripcion,Responsable,DireccionAnyDesk,DireccionTeamViewer,IpEquipo,IpPublica,IpLocal,TipoEquipo,UsuarioEquipo,ClaveEquipo")] Equipos equipos)
        {
            if (ModelState.IsValid)
            {
                db.Equipos.Add(equipos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(equipos);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Equipos equipos = db.Equipos.Find(id);
            if (equipos == null)
            {
                return HttpNotFound();
            }
            return View(equipos);
        }

        // POST: Equipos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdEquipo,IdCliente,Descripcion,Responsable,DireccionAnyDesk,DireccionTeamViewer,IpEquipo,IpPublica,IpLocal,TipoEquipo,UsuarioEquipo,ClaveEquipo")] Equipos equipos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(equipos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(equipos);
        }

        // GET: Equipos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Equipos equipos = db.Equipos.Find(id);
            if (equipos == null)
            {
                return HttpNotFound();
            }
            return View(equipos);
        }

        // POST: Equipos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Equipos equipos = db.Equipos.Find(id);
            db.Equipos.Remove(equipos);
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
    }
}
