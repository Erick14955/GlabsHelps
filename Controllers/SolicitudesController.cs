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
    [Authorize]
    public class SolicitudesController : Controller
    {
        public ActionResult Index()
        {
            return View(Solicitudes.SolicitudesList());
        }

        [HttpPost]
        public ActionResult Index(string busqueda)
        {
            var solic = from s in Solicitudes.SolicitudesList() select s;
            if (!string.IsNullOrEmpty(busqueda))
            {
                return View(solic.Where(s => s.Descripcion.Contains(busqueda) || s.Detalle.Contains(busqueda)));
            }
            else
            {
                return View(Solicitudes.SolicitudesList());
            }
        }

        public ActionResult Details(Decimal id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Solicitudes solic = new Solicitudes(id);
            if (solic.IdSolicitud == 0)
            {
                return HttpNotFound();
            }
            return View(solic);
        }

        public ActionResult Create()
        {
            var cli = from s in Clientes.ClienteList() select s;
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var clien in cli)
            {
                items.Add(new SelectListItem
                {
                    Value = clien.IdCliente.ToString(),
                    Text = clien.Nombre,
                    Selected = false
                });
            }

            ViewBag.items = items;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdSolicitud,Descripcion,Detalle,Fecha,FechaFinalizado,Estatus,IdCliente,IdUsuario,IdIngeniero,FechaAgenda")] Solicitudes solic)
        {
            if (ModelState.IsValid)
            {
                solic.Guardar();
                return RedirectToAction("Index");
            }

            return View(solic);
        }

        public ActionResult Edit(Decimal id)
        {
            var cli = from s in Clientes.ClienteList() select s;
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var clien in cli)
            {
                items.Add(new SelectListItem
                {
                    Value = clien.IdCliente.ToString(),
                    Text = clien.Nombre,
                    Selected = false
                });
            }

            ViewBag.items = items;

            List<SelectListItem> tipos = new List<SelectListItem>()
            {
                new SelectListItem { Value = "1", Text ="No iniciado", Selected = false },
                new SelectListItem { Value = "2", Text = "Iniciado", Selected = false },
                new SelectListItem { Value = "3", Text = "Terminado", Selected = false }
            };

            ViewBag.tipos = tipos;
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Solicitudes solic = new Solicitudes(id);
            if (solic.IdSolicitud == 0)
            {
                return HttpNotFound();
            }
            return View(solic);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdSolicitud,Descripcion,Detalle,Fecha,FechaFinalizado,Estatus,IdCliente,IdUsuario,IdIngeniero,FechaAgenda")] Solicitudes solic)
        {
            if (ModelState.IsValid)
            {
                solic.Guardar();
                return RedirectToAction("Index");
            }
            return View(solic);
        }

        public ActionResult Delete(Decimal id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Solicitudes solic = new Solicitudes(id);
            if (solic.IdSolicitud == 0)
            {
                return HttpNotFound();
            }
            return View(solic);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Decimal id)
        {
            Solicitudes solic = new Solicitudes(id);
            solic.Eliminar();
            return RedirectToAction("Index");
        }
    }
}
