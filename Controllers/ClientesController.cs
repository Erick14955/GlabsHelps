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
    public class ClientesController : Controller
    {
        private GlabsHelpsContext db = new GlabsHelpsContext();

        public ActionResult Index(string busqueda)
        {
            ViewData["CurrentFilter"] = busqueda;
            var clientes = from s in db.Clientes select s;

            if (!string.IsNullOrEmpty(busqueda))
            {
                clientes = clientes.Where(s => s.IdCliente.ToString().Contains(busqueda) || s.Nombre.Contains(busqueda) || s.Direccion.Contains(busqueda)
                || s.Telefono.ToString().Contains(busqueda) || s.CorreoCliente.Contains(busqueda) || s.Contacto.Contains(busqueda)
                || s.CelularContacto.Contains(busqueda) || s.CorreoContacto.Contains(busqueda));
            }
            return View(clientes.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Clientes clientes = db.Clientes.Find(id);
            if (clientes == null)
            {
                return HttpNotFound();
            }
            return View(clientes);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdCliente,Nombre,Direccion,Telefono,CorreoCliente,Contacto,CelularContacto,CorreoContacto")] Clientes clientes)
        {
            if (ModelState.IsValid)
            {
                db.Clientes.Add(clientes);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(clientes);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Clientes clientes = db.Clientes.Find(id);
            if (clientes == null)
            {
                return HttpNotFound();
            }
            return View(clientes);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdCliente,Nombre,Direccion,Telefono,CorreoCliente,Contacto,CelularContacto,CorreoContacto")] Clientes clientes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(clientes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(clientes);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Clientes clientes = db.Clientes.Find(id);
            if (clientes == null)
            {
                return HttpNotFound();
            }
            return View(clientes);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Clientes clientes = db.Clientes.Find(id);
            db.Clientes.Remove(clientes);
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
